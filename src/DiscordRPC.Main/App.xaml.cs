using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;
using DiscordRPC.Main.Properties;

namespace DiscordRPC.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 

    public partial class App : Application
    {
        // Debug only
        static string TAG = "App.xaml.cs: ";

        // Classes
        GetDiscordProcess getDiscordProcess = new GetDiscordProcess();

        private static Mutex mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            JsonConfig.LoadJson();

            // Scan for Discord Process
            getDiscordProcess.DiscordProcessName();

            string app_name = "DiscordRPC.Main";
            bool createdNew;
            mutex = new Mutex(true, app_name, out createdNew);


            if (!createdNew)
            {       
                MessageBox.Show("Application is already running", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.OK, MessageBoxImage.Hand);
                Debug.WriteLine(TAG + "Application is already running");
                Application.Current.Shutdown();
            }
            else
            {

                if (getDiscordProcess.IsDiscordRunning == true)
                {
                    // If Discord process is running, start application
                    if (JsonConfig.settings.firstRun == true)
                    {
                        FirstRunWindow first = new FirstRunWindow();
                        first.Show();
                        Debug.WriteLine(TAG + "Launching FirstRunWindow.xaml");
                    }
                    else if (Settings.Default.app_first_run == false)
                    {
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        Debug.WriteLine(TAG + "Launching MainWindow.xaml");
                    }
                }
                else if (getDiscordProcess.IsDiscordRunning == false)
                {
                    // Shutdown application if Discord process is not running
                    MessageBox.Show("Discord is not running.", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    Debug.WriteLine(TAG + "Discord is not running. Shutting down application");
                    Application.Current.Shutdown();
                }

                base.OnStartup(e);
            }

        }


    }
}
