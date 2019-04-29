using System;
using System.Diagnostics;
using System.Windows;


namespace DiscordRPC.Main
{
    /// <summary>
    /// Interaction logic for FirstRunWindow.xaml
    /// </summary>
    public partial class FirstRunWindow : Window
    {

        // Debug only
        static string TAG = "FirstRunWindow.xaml: ";

        public FirstRunWindow()
        {
            InitializeComponent();
            this.Closed += ExitApplication;

        }

        private void ExitApplication(object sender, EventArgs e)
        {
            Application.Current.Shutdown();

        }

        private void ButtonEndTutorial_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            JsonConfig.settings.firstRun = false;
            JsonConfig.SaveJson();

            this.Hide();
        }

        private void ButtonDiscordDeveloperPortal_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discordapp.com/developers/applications/");
        }
    }
}
