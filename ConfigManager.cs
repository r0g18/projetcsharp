using System;
using System.IO;
using Newtonsoft.Json;

public class ConfigManager
{
    public string ApiKey { get; set; }
    public string DefaultCity { get; set; }
    public string Language { get; set; }

    private const string ConfigFilePath = "config.json";
    private const string OptionsFilePath = "options.json";

    public static ConfigManager LoadConfig()
    {
        if (!File.Exists(ConfigFilePath))
        {
            throw new FileNotFoundException("Le fichier 'config.json' est introuvable. Vérifiez son emplacement.");
        }

        try
        {
            string configContent = File.ReadAllText(ConfigFilePath);
            return JsonConvert.DeserializeObject<ConfigManager>(configContent);
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("Erreur lors de la lecture du fichier de configuration : " + ex.Message);
        }
    }

    public void SaveOptions()
    {
        var options = new
        {
            DefaultCity,
            Language
        };

        try
        {
            string optionsJson = JsonConvert.SerializeObject(options, Formatting.Indented);
            File.WriteAllText(OptionsFilePath, optionsJson);
        }
        catch (IOException ex)
        {
            Console.WriteLine("Erreur lors de l'écriture des options : " + ex.Message);
        }
    }

    public void LoadOptions()
    {
        if (File.Exists(OptionsFilePath))
        {
            try
            {
                string optionsContent = File.ReadAllText(OptionsFilePath);
                var options = JsonConvert.DeserializeObject<ConfigManager>(optionsContent);

                DefaultCity = options.DefaultCity;
                Language = options.Language;
            }
            catch (JsonException ex)
            {
                Console.WriteLine("Erreur lors de la lecture des options : " + ex.Message);
            }
        }
        else
        {
            Console.WriteLine("Aucune option sauvegardée trouvée.");
        }
    }
}
