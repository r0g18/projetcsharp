using System;
using System.Windows;

public partial class MainWindow : Window
{
    private readonly ConfigManager config;
    private readonly WeatherService weatherService;

    public MainWindow()
    {
        InitializeComponent();

        config = ConfigManager.LoadConfig();
        config.LoadOptions();
        weatherService = new WeatherService(config.ApiKey);

        SearchButton.Click += async (_, _) =>
        {
            var city = CityInput.Text;

            try
            {
                var weatherInfo = await weatherService.GetWeatherByCity(city);
                WeatherOutput.Text = weatherInfo;
            }
            catch (Exception ex)
            {
                WeatherOutput.Text = $"Erreur : {ex.Message}";
            }
        };

        SaveSettings.Click += (_, _) =>
        {
            try
            {
                config.DefaultCity = DefaultCity.Text;
                config.Language = LanguageSelector.SelectedItem.ToString();
                config.SaveOptions();
                MessageBox.Show("Options sauvegardées avec succès.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de la sauvegarde : {ex.Message}");
            }
        };

        ForecastButton.Click += async (_, _) =>
        {
            var city = CityInput.Text;

            try
            {
                var forecastInfo = await weatherService.GetForecastByCity(city);
                WeatherOutput.Text = forecastInfo;
            }
            catch (Exception ex)
            {
                WeatherOutput.Text = $"Erreur : {ex.Message}";
            }
        };
    }
}
