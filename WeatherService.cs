public static class WeatherService
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task<string> GetWeatherAsync(string city)
    {
        var apiKey = "your-api-key-here";  // Remplace par ta clé API
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric";
        
        var response = await client.GetStringAsync(url);
        var weatherData = JsonConvert.DeserializeObject<dynamic>(response);

        return $"Météo à {city}: {weatherData.main.temp}°C, {weatherData.weather[0].description}";
    }
}
