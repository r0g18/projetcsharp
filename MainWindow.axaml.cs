using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

public class WeatherService
{
    private readonly string apiKey;

    public WeatherService(string apiKey)
    {
        this.apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey), "La clé API ne peut pas être nulle.");
    }

    public async Task<string> GetWeatherByCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return "Le nom de la ville ne peut pas être vide.";
        }

        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&lang=fr&appid={apiKey}";

        try
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return "Ville introuvable ou problème réseau.";
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);

            var cityName = data["name"]?.ToString() ?? "Inconnu";
            var latitude = data["coord"]?["lat"]?.ToString() ?? "N/A";
            var longitude = data["coord"]?["lon"]?.ToString() ?? "N/A";
            var temperature = data["main"]?["temp"]?.ToString() ?? "N/A";
            var humidity = data["main"]?["humidity"]?.ToString() ?? "N/A";
            var description = data["weather"]?[0]?["description"]?.ToString() ?? "N/A";

            return $"Ville : {cityName}\n" +
                   $"Latitude : {latitude}\n" +
                   $"Longitude : {longitude}\n" +
                   $"Température : {temperature}°C\n" +
                   $"Humidité : {humidity}%\n" +
                   $"Temps : {description}";
        }
        catch (HttpRequestException ex)
        {
            return $"Erreur lors de la connexion au service météo : {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Une erreur inattendue s'est produite : {ex.Message}";
        }
    }
}
