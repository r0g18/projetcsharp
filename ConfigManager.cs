using System;
using System.IO;
using Newtonsoft.Json;

public class ConfigManager
{
    public static string GetApiKey()
    {
        string path = "config.json";
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            dynamic config = JsonConvert.DeserializeObject(json);
            return config.ApiKey;
        }
        else
        {
            throw new Exception("Le fichier config.json est manquant.");
        }
    }
}
