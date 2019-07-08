using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace DiscordRPC.Main
{
    public class AppConfig
    {
        public string discordUsername { get; set; }
        public string discordAvatarSmall { get; set; }
        public string discordAvatarMedium { get; set; }
        public string discordAvatarLarge { get; set; }
        public string discordClientId { get; set; }
        public string discordClientIdHash { get; set; }
        public string discordPresenceState { get; set; }
        public string discordPresenceDetail { get; set; }
        public string discordLargeImageKey { get; set; }
        public string discordLargeImageText { get; set; }
        public string discordSmallImageKey { get; set; }
        public string discordSmallImageText { get; set; }

        public bool firstRun = true; // Default value
    }
    class JsonConfig
    {
        private static string fileName = "pref.json";
        private static bool firstRun = !File.Exists(fileName);
        public static AppConfig settings = new AppConfig();

        public static void LoadJson()
        {
            if (!firstRun)
            {
                // Read existing JSON file
                string jsonText = File.ReadAllText(fileName);
                settings = JsonConvert.DeserializeObject<AppConfig>(jsonText);
                Debug.WriteLine(jsonText);
            }
            else
            {
                // Create empty JSON file 
                string jsonFormat = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(fileName, jsonFormat);
            }

        }

        public static async void SaveJson()
        {
            await Task.Run(() =>
            {
                try
                {
                    string jsonText = JsonConvert.SerializeObject(settings, Formatting.Indented);
                    File.WriteAllText(fileName, jsonText);
                }
                catch(IOException e)
                {
                    MessageBox.Show(e.ToString(),
                    Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title,
                    MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });

        }

    }
}
