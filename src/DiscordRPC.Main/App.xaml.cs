using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace DiscordRPC.Main
{
    public partial class App : Application
    {
        // Debug only
        private readonly string TAG = "App.xaml.cs: ";

        private static Mutex mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            LocalizationManager.SetApplicationLanguage();
            JsonConfig.LoadJson();
            AppUpdateChecker.CheckVersion();
            DiscordProcessListener.DiscordProcessName();

            string app_name = "DiscordRPC.Main";
            bool createdNew;
            mutex = new Mutex(true, app_name, out createdNew);

            if (!createdNew)
            {       
                MessageBox.Show((string)Application.Current.FindResource("app_error_application_second_instance"), 
                    Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, 
                    MessageBoxButton.OK, MessageBoxImage.Hand);
                Debug.WriteLine(TAG + "Application is already running");
                Application.Current.Shutdown();
            }
            else
            {
#if DEBUG
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                Debug.WriteLine(TAG + "Launching MainWindow.xaml (Debug)");
#else
                if (DiscordProcessListener.IsDiscordRunning == true)
                {
                    // If Discord process is running, start application
                    if (JsonConfig.settings.firstRun == true)
                    {
                        FirstRunWindow first = new FirstRunWindow();
                        first.Show();
                        Debug.WriteLine(TAG + "Launching FirstRunWindow.xaml (Release)");
                    }
                    else if (JsonConfig.settings.firstRun == false)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Debug.WriteLine(TAG + "Launching MainWindow.xaml (Release)");
                    }
                }
                else if (DiscordProcessListener.IsDiscordRunning == false)
                {
                    // Shutdown application if Discord process is not running
                    MessageBox.Show((string)Application.Current.FindResource("app_error_discord_not_running"), 
                        Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    Debug.WriteLine(TAG + "Discord is not running. Shutting down application");
                    Application.Current.Shutdown();
                }

                base.OnStartup(e);
#endif
            }

        }

        }
    }
