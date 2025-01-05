using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Avalonia.Controls;

namespace MeteoApp
{
    public partial class MainWindow : Window
    {
        private const string ConfigFile = "config.json";
        private string _apiKey;

        public MainWindow()
        {
            InitializeComponent();
            LoadApiKey();
        }

        private void LoadApiKey()
        {
            if (!System.IO.File.Exists(ConfigFile))
            {
                ResultTextBlock.Text = "Erreur : Fichier config.json introuvable.";
                return;
            }

            var config = JsonConvert.DeserializeObject<dynamic>(System.IO.File.ReadAllText(ConfigFile));
            _apiKey = config.ApiKey;
        }

        private async void OnSearchWeatherClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var city = CityTextBox.Text;
            if (string.IsNullOrWhiteSpace(city))
            {
                ResultTextBlock.Text = "Veuillez entrer une ville.";
                return;
            }

            await FetchWeatherAsync(city);
        }

        public async Task FetchWeatherAsync(string city)
        {
            try
            {
                using var client = new HttpClient();
                var response = await client.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric&lang=fr");
                dynamic weatherData = JsonConvert.DeserializeObject(response);

                string result = $"Ville : {weatherData.name}\n" +
                                $"Latitude : {weatherData.coord.lat}, Longitude : {weatherData.coord.lon}\n" +
                                $"Température : {weatherData.main.temp} °C\n" +
                                $"Humidité : {weatherData.main.humidity}%\n" +
                                $"Description : {weatherData.weather[0].description}";

                ResultTextBlock.Text = result;
            }
            catch (Exception ex)
            {
                ResultTextBlock.Text = $"Erreur : {ex.Message}";
            }
        }
    }
}
