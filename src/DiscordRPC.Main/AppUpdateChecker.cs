using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using RestSharp;

namespace DiscordRPC.Main
{
    public class GithubApi
    {
        public string tag_name { get; set; }

    }
    public class AppUpdateChecker
    {
        private static string github_api = "https://api.github.com";
        private static string github_api_request = "/repos/ddasutein/Discord-RPC-csharp/releases/latest";
        private readonly static string TAG = "AppUpdateChecker.cs: ";
        private readonly static string AppTitle = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
        public static bool IsUpdateAvailable { get; set; }

        public static void CheckVersion()
        {
            try
            {
                Version latest_version = new Version(GetLatestRelease());
                Version current_version = new Version(GetCurrentApplicationVersion());

                if (current_version < latest_version)
                {
                    IsUpdateAvailable = true;
                    MessageBoxResult result = MessageBox.Show(
                        (string)Application.Current.FindResource("app_update_title_available_description"),
                        (string)Application.Current.FindResource("app_update_title_available"),
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                    {
                        Process.Start("https://github.com/ddasutein/Discord-RPC-csharp/releases");
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    IsUpdateAvailable = false;
#if DEBUG
                    Debug.WriteLine(TAG + "Application is using the latest version");
#endif
                }
            }
            catch (Exception e)
            {
                MessageBox.Show((string)Application.Current.FindResource("app_error_update_no_internet_connection"),
                    (string)Application.Current.FindResource("app_error_update_no_internet_connection_title"), 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }

        }
        private static string GetLatestRelease()
        {
            var client = new RestClient(github_api);
            var request = new RestRequest(github_api_request);
            request.AddHeader("Content-Type", "application/json;");
            var response = client.Execute<GithubApi>(request);

            if (!response.IsSuccessful)
            {
                return null;
            }

            return response.Data.tag_name.Substring(1);
        }
        public static string GetCurrentApplicationVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }

        public static void ManualCheckVersion()
        {
            if (!IsUpdateAvailable)
            {
                MessageBox.Show(
                    (string)Application.Current.FindResource("app_update_title_not_available_description"),
                    (string)Application.Current.FindResource("app_update_title_not_available"), 
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBoxResult result = MessageBox.Show(
                (string)Application.Current.FindResource("app_update_title_available_description"),
                (string)Application.Current.FindResource("app_update_title_available"),
                 MessageBoxButton.YesNo,
                 MessageBoxImage.Information);
                if (result == MessageBoxResult.Yes)
                {
                    Process.Start("https://github.com/ddasutein/Discord-RPC-csharp/releases");
                }
                else
                {
                    return;
                }
            }
        }
    }
}
