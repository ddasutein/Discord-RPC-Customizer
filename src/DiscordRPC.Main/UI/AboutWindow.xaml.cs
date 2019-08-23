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
            if (!IsRunningAsUWP)
            {
                this.ProjectBuildType.Content = (string)Application.Current.FindResource("aw_label_application_build_type_win32");
            }
            else
            {
                this.ProjectBuildType.Content = (string)Application.Current.FindResource("aw_label_application_build_type_uwp");
            }
        }

        private void GetApplicationVersionNumber()
        {
            var version = AppUpdateChecker.GetCurrentApplicationVersion();
#if DEBUG
            textBlockVersionNumber.Content = version.Remove(version.Length - 2) + "-dev_build";
#else
            textBlockVersionNumber.Content = version.Remove(version.Length - 2);
#endif

#if DEBUG
            Debug.WriteLine(TAG + "RikoRPC version is: " + version);
#endif
        }

        private void GetDiscordBuildType()
        {
            textBlockDiscordBuildType.Content = DiscordProcessListener.DiscordBuildType;
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

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
