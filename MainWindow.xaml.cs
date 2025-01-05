public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        GetWeatherButton.Click += OnGetWeatherButtonClicked;
    }

    private async void OnGetWeatherButtonClicked(object sender, RoutedEventArgs e)
    {
        var city = CityTextBox.Text?.Trim();
        if (string.IsNullOrEmpty(city))
        {
            WeatherTextBlock.Text = "Veuillez entrer un nom de ville.";
            return;
        }

        try
        {
            var weather = await WeatherService.GetWeatherAsync(city);
            WeatherTextBlock.Text = weather;
        }
        catch (Exception ex)
        {
            WeatherTextBlock.Text = $"Erreur : {ex.Message}";
        }
    }
}
