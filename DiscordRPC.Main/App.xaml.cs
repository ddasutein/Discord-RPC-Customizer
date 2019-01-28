using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DiscordRPC.Main.Properties;

namespace DiscordRPC.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        // Debug only
#pragma warning disable CS0414 // The field 'App.TAG' is assigned but its value is never used
        static string TAG = "App.xaml.cs: ";
#pragma warning restore CS0414 // The field 'App.TAG' is assigned but its value is never used

        private static Mutex mutex = null;
#pragma warning disable CS0414 // The field 'App.isFirstTimeOpened' is assigned but its value is never used
        private static bool isFirstTimeOpened = true;
#pragma warning restore CS0414 // The field 'App.isFirstTimeOpened' is assigned but its value is never used

        protected override void OnStartup(StartupEventArgs e)
        {
            const string app_name = "DiscordRPC.Main";
            bool createdNew;

            mutex = new Mutex(true, app_name, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("Application is already running", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.OK, MessageBoxImage.Hand);
                Application.Current.Shutdown();
            }
            else
            {

                if (Settings.Default.app_first_run == true)
                {
                    FirstRunWindow first = new FirstRunWindow();
                    first.Show();
                }
                else if (Settings.Default.app_first_run == false){
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                }

                base.OnStartup(e);
            }

        }


    }
}
