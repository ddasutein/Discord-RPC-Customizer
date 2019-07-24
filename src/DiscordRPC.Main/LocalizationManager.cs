using System;
using System.Globalization;
using System.Windows;

namespace DiscordRPC.Main
{
    /* Code sample from: https://stackoverflow.com/questions/12584781/localization-of-wpf-application */
    public class LocalizationManager
    {
        public static void SetApplicationLanguage()
        {
            ResourceDictionary resourceDictionary = new ResourceDictionary();
            string lang = "en-US"; // placeholder 
            switch (lang)
            {
                case "en-US":
                    resourceDictionary.Source = new Uri("..\\Languages\\StringResources.xaml", UriKind.Relative);
                    break;
                case "ja-JP":
                    resourceDictionary.Source = new Uri("..\\Languages\\StringResources_ja-JP.xaml", UriKind.Relative);
                    break;
                default:
                    resourceDictionary.Source = new Uri("..\\Languages\\StringResources.xaml", UriKind.Relative);
                    break;
            }

            Application.Current.Resources.MergedDictionaries.Add(resourceDictionary);
        }

        private string GetCultureInfo()
        {
            return CultureInfo.CurrentCulture.Name;
        }

    }
}
