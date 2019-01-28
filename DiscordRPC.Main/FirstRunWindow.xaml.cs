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

        private bool setFirstTimeOpenBool = false;

        public FirstRunWindow()
        {
            InitializeComponent();

            // If application version is updated, move user settings to new version
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Debug.WriteLine(TAG + "User settings upgraded");
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

        }

        private void ButtonEndTutorial_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            Properties.Settings.Default.app_first_run = setFirstTimeOpenBool;
            Properties.Settings.Default.Save();

            this.Close();
        }
    }
}
