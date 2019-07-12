using System.Diagnostics;
using System.Windows;

namespace DiscordRPC.Main
{
    public partial class AboutWindow : Window
    {

        // Debug only
        private readonly string TAG = "AboutWindow.xaml: ";
        private bool IsRunningAsUWP = false; // To be changed later

        public AboutWindow()
        {
            InitializeComponent();
            GetApplicationVersionNumber();
            GetDiscordBuildType();
            
            // Temporary code for the future
            this.ProjectBuildType.Visibility = Visibility.Collapsed;
        }

        private void GetApplicationVersionNumber()
        {
            var version = AppUpdateChecker.GetCurrentApplicationVersion();
            textBlockVersionNumber.Text = "Version: " + version.Remove(version.Length - 2);
#if DEBUG
            Debug.WriteLine(TAG + "RikoRPC version is: " + version);
#endif
        }

        private void GetDiscordBuildType()
        {
            textBlockDiscordBuildType.Text = DiscordProcessListener.DiscordBuildType;
        }
        private void buttonGitHub_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/ddasutein/Discord-RPC-csharp");
        }

        private void Button_Check_For_Updates_Click(object sender, RoutedEventArgs e)
        {
            if (!IsRunningAsUWP)
            {
                AppUpdateChecker.ManualCheckVersion();
            } else
            {
                Process.Start("ms-windows-store:");
            }

        }
    }
}
