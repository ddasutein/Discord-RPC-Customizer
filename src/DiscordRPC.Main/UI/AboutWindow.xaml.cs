using System.Diagnostics;
using System.Windows;

namespace DiscordRPC.Main
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {

        // Debug only
        private string TAG = "AboutWindow.xaml: ";

        // Global variables
        GetDiscordProcess getDiscordProcess = new GetDiscordProcess();

        public AboutWindow()
        {
            InitializeComponent();

            // Show application version
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            textBlockVersionNumber.Text = "Version: " + version.Remove(version.Length - 2);
            Debug.WriteLine(TAG + "DiscordRPC version is: " + version);

            // Run Discord process check then display to text block
            getDiscordProcess.DiscordProcessName();

            #if DEBUG
            if (getDiscordProcess.DiscordBuildInfo == null)
            {
                textBlockDiscordBuildType.Text = "(null)";
            }
            else
            {
                textBlockDiscordBuildType.Text = getDiscordProcess.DiscordBuildInfo.ToString();
            }
            #else
                textBlockDiscordBuildType.Text = getDiscordProcess.DiscordBuildInfo.ToString();
            #endif
        }

        private void buttonGitHub_Click(object sender, RoutedEventArgs e)
        {
            // Sends user to GitHub repo
            var url = "https://github.com/ddasutein/Discord-RPC-csharp";
            Process.Start(url);
            Debug.WriteLine(TAG + url);
        }
    }
}
