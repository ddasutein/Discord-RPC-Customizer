using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace DiscordRPC.Main
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        private static Mutex mutex = null;

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

                base.OnStartup(e);
            }

        }

    }
}
