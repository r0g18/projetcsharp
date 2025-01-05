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
            var temperature = data["main"]?["temp"]?.ToString() ?? "N/A";
            var description = data["weather"]?[0]?["description"]?.ToString() ?? "N/A";

            return $"Ville : {cityName}\nTempérature : {temperature}°C\nTemps : {description}";
        }
        catch (HttpRequestException ex)
        {
            return $"Erreur réseau : {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Une erreur inattendue s'est produite : {ex.Message}";
        }
    }
    public async Task<string> GetForecastByCity(string city)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            return "Le nom de la ville ne peut pas être vide.";
        }

        var url = $"https://api.openweathermap.org/data/2.5/forecast?q={city}&units=metric&lang=fr&appid={apiKey}";

        try
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                return "Impossible de récupérer les prévisions.";
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);

            var forecast = "Prévisions sur 5 jours à 12:00 :\n";
            foreach (var item in data["list"])
            {
                var date = item["dt_txt"].ToString();
                if (date.Contains("12:00:00"))
                {
                    var temp = item["main"]?["temp"]?.ToString() ?? "N/A";
                    var desc = item["weather"]?[0]?["description"]?.ToString() ?? "N/A";
                    forecast += $"\nDate : {date}\nTempérature : {temp}°C\nTemps : {desc}\n";
                }
            }

            return forecast;
        }
        catch (HttpRequestException ex)
        {
            return $"Erreur réseau : {ex.Message}";
        }
        catch (Exception ex)
        {
            return $"Une erreur inattendue s'est produite : {ex.Message}";
        }
    }
}
