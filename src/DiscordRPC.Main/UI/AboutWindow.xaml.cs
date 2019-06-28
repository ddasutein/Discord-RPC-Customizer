using System.Diagnostics;
using System.Windows;

namespace DiscordRPC.Main
{
    public partial class AboutWindow : Window
    {

        // Debug only
        private readonly string TAG = "AboutWindow.xaml: ";

        public AboutWindow()
        {
            InitializeComponent();
            GetApplicationVersionNumber();
            GetDiscordBuildType();
        }

        private void GetApplicationVersionNumber()
        {
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            textBlockVersionNumber.Text = "Version: " + version.Remove(version.Length - 2);
#if DEBUG
            Debug.WriteLine(TAG + "RikoRPC version is: " + version);
#endif
        }

        private void GetDiscordBuildType()
        {
            DiscordProcessListener getDiscordProcess = new DiscordProcessListener();
            getDiscordProcess.DiscordProcessName();
            textBlockDiscordBuildType.Text = getDiscordProcess.DiscordBuildInfo;
        }
        private void buttonGitHub_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/ddasutein/Discord-RPC-csharp");
        }
    }
}
