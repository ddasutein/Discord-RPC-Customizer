using System;
using System.Windows;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Media.Imaging;

namespace DiscordRPC.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// https://github.com/discordapp/discord-rpc/blob/master/examples/button-clicker/Assets/DiscordController.cs
    /// </summary>
    /// 

    public partial class MainWindow : Window 
    {

        // Debug only
        static string TAG = "MainWindow.xaml: ";

        // Global Variables
        private bool isDiscordPresenceRunning = false;
        private bool isTimeStampEnabled = false;

        // Classes
        GetSpotifyProcess getSpotifyProcess = new GetSpotifyProcess();
        JumpListManager jumpListManager = new JumpListManager();
        ResetApplication resetApplication = new ResetApplication();
        PresenceManager presenceManager = new PresenceManager();
        HashManager hashManager = new HashManager();

        // Threads
        Thread spotifyProcessScanThread;
        Thread StartHashManagerThread;

        public MainWindow()
        {
            // Load most important stuff here
            InitializeComponent();
            this.Closed += ExitApplication;
            jumpListManager.LoadJumpLists();
            LoadUserStatePresence();

            // Start threads
            spotifyProcessScanThread = new Thread(new ThreadStart(getSpotifyProcess.SpotifyProcess));
            spotifyProcessScanThread.IsBackground = true;
            spotifyProcessScanThread.Start();

            StartDiscordPresence();

        }

        void ExitApplication(object sender, EventArgs e)
        {
            // When user closes the application, the application will dispose the Discord RPC client.
            if (isDiscordPresenceRunning == true)
            {
                presenceManager.ShutdownPresence();
                Application.Current.Shutdown();
            }
            else
            {
                Application.Current.Shutdown();
            }

        }
        private void StartDiscordPresence()
        {

            if (String.IsNullOrEmpty(TextBox_clientId.Password))
            {
                this.SetStatusBarMessage("Discord RPC is offline.");;
                this.Button_Update.IsEnabled = false;
                this.Button_Shutdown.IsEnabled = false;
                this.Button_Initialize_Discord.IsEnabled = true;
                statusIconImage.Source = new BitmapImage(new Uri("/DiscordRPCMain;component/Resources/icons8_offline.png", UriKind.Relative));
                return;
            }
            else
            {
                this.SetStatusBarMessage("Connecting to Discord...");

                hashManager.discordClientId = this.TextBox_clientId.Password;
                StartHashManagerThread = new Thread(new ThreadStart(hashManager.HashId));
                StartHashManagerThread.IsBackground = true;
                StartHashManagerThread.Start();
                //presenceManager.InitializeDiscordRPC();

                this.Button_Initialize_Discord.IsEnabled = false;
                this.Button_Update.IsEnabled = true;
                this.Button_Shutdown.IsEnabled = true;
                this.Button_afk_and_lock_pc.IsEnabled = true;
                this.TextBox_clientId.IsEnabled = false;

                statusIconImage.Source = new BitmapImage(new Uri("/DiscordRPCMain;component/Resources/icons8_online.png", UriKind.Relative));
                isDiscordPresenceRunning = true;
                this.SetStatusBarMessage("Discord RPC is online");

                this.usernameLabel.Content = JsonConfig.settings.discordUsername;
                this.discordAvatarImage.Source = new BitmapImage(new Uri(JsonConfig.settings.discordAvatarUri));
            }
        }

        private void updatePresence()
        {

            #if DEBUG
            Debug.WriteLine(TAG + "Details: " + TextBox_details.Text);
            Debug.WriteLine(TAG + "State: " + TextBox_state.Text);
            Debug.WriteLine(TAG + "LargeImageKey: " + TextBox_largeImageKey.Text);
            Debug.WriteLine(TAG + "LargeImageText: " + TextBox_largeImageText.Text);
            Debug.WriteLine(TAG + "SmallImageKey: " + TextBox_smallImageKey.Text);
            Debug.WriteLine(TAG + "SmallImageText: " + TextBox_smallImageText.Text);
            Debug.WriteLine(TAG + "Updated presence and settings");
            #endif

            // Show MessageBox to notify user Spotify client is running
            if (getSpotifyProcess.IsSpotifyOpened)
            {
                MessageBox.Show("Discord RPC has detected Spotify is running. Your rich presence or Spotify presence will not update until your RPC client or Spotify client is offline.", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            if (!isTimeStampEnabled)
            {
                presenceManager.useTimeStamp = isTimeStampEnabled;
                presenceManager.discordPresenceDetail = this.TextBox_details.Text;
                presenceManager.discordPresenceState = this.TextBox_state.Text;
                presenceManager.discordLargeImageKey = this.TextBox_largeImageKey.Text;
                presenceManager.discordLargeImageText = this.TextBox_largeImageText.Text;
                presenceManager.discordSmallImageKey = this.TextBox_smallImageKey.Text;
                presenceManager.discordSmallImageText = this.TextBox_smallImageText.Text;
                presenceManager.UpdatePresence();
            }
            else
            {
                presenceManager.useTimeStamp = isTimeStampEnabled;
                presenceManager.discordPresenceDetail = this.TextBox_details.Text;
                presenceManager.discordPresenceState = this.TextBox_state.Text;
                presenceManager.discordLargeImageKey = this.TextBox_largeImageKey.Text;
                presenceManager.discordLargeImageText = this.TextBox_largeImageText.Text;
                presenceManager.discordSmallImageKey = this.TextBox_smallImageKey.Text;
                presenceManager.discordSmallImageText = this.TextBox_smallImageText.Text;
                presenceManager.UpdatePresence();
            }

            SaveUserStatePresence();

        }

        private void LoadUserStatePresence()
        {;
            this.TextBox_clientId.Password = JsonConfig.settings.discordClientId;
            this.TextBox_state.Text = JsonConfig.settings.discordPresenceState;
            this.TextBox_details.Text = JsonConfig.settings.discordPresenceDetail;
            this.TextBox_largeImageKey.Text = JsonConfig.settings.discordLargeImageKey;
            this.TextBox_largeImageText.Text = JsonConfig.settings.discordLargeImageText;
            this.TextBox_smallImageKey.Text = JsonConfig.settings.discordSmallImageKey;
            this.TextBox_smallImageText.Text = JsonConfig.settings.discordSmallImageText;
        }

        private void SaveUserStatePresence()
        {

            JsonConfig.settings.discordClientId = this.TextBox_clientId.Password;
            JsonConfig.settings.discordPresenceState = this.TextBox_state.Text;
            JsonConfig.settings.discordPresenceDetail = this.TextBox_details.Text;
            JsonConfig.settings.discordLargeImageKey = this.TextBox_largeImageKey.Text;
            JsonConfig.settings.discordLargeImageText = this.TextBox_largeImageText.Text;
            JsonConfig.settings.discordSmallImageKey = this.TextBox_smallImageKey.Text;
            JsonConfig.settings.discordSmallImageText = this.TextBox_smallImageText.Text;
            JsonConfig.SaveJson();
            Debug.WriteLine(TAG + "Settings saved");

        }

        /// <summary>
        /// Stop RPC.
        /// </summary>
        private void Shutdown()
        {

            if (MessageBox.Show("Are you sure you want to go offline?", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                presenceManager.ShutdownPresence();
                this.SetStatusBarMessage("Discord RPC is offline.");
                this.usernameLabel.Content = "Not found";
                this.Button_Update.IsEnabled = false;
                this.Button_Shutdown.IsEnabled = false;
                this.Button_Initialize_Discord.IsEnabled = true;
                this.TextBox_clientId.IsEnabled = true;
                this.Button_afk_and_lock_pc.IsEnabled = false;
                statusIconImage.Source = new BitmapImage(new Uri("/DiscordRPCMain;component/Resources/icons8_offline.png", UriKind.Relative));
            }
            else
            {
                // Do nothing
                return;
            }

        }

        /// <summary>
        /// Just set a message to be displayed in the status bar at the window's bottom.
        /// </summary>
        /// <param name="message"></param>
        private void SetStatusBarMessage(string message)
        {
            this.Label_Status.Content = message;
        }

        String clientID;

        private void Button_Initialize_Click(object sender, RoutedEventArgs e)
        {
            clientID = this.TextBox_clientId.Password.ToString();
            bool isNumeric = ulong.TryParse(clientID, out ulong n);

            if (!isNumeric)
            {
                MessageBox.Show("The client ID is either empty or not a numeric value.", "Client ID Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else
            {
                SaveUserStatePresence();
                StartDiscordPresence();
            }
        }

        private void Button_save_settings_Click(object sender, RoutedEventArgs e)
        {
            SaveUserStatePresence();
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            updatePresence();
        }

        private void Button_Shutdown_Click(object sender, RoutedEventArgs e)
        {
            Shutdown();
        }

        private void Button_open_discord_api(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discordapp.com/developers/applications/me" + "/" + TextBox_clientId.Password);
        }

        private void Button_reset_app(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Do you want to reset this application? Application will close after a reset.", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                resetApplication.deleteConfig();
            }
            else
            {
                // Do nothing
                return;
            }

        }

        private void checkBoxTimeStamp_Checked(object sender, RoutedEventArgs e)
        {
            isTimeStampEnabled = true;
        }

        private void checkBoxTimeStamp_Unchecked(object sender, RoutedEventArgs e)
        {
            isTimeStampEnabled = false;
        }

        private void buttonAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void Button_afk_and_lock_pc_Click(object sender, RoutedEventArgs e)
        {
            presenceManager.useTimeStamp = true;
            presenceManager.discordPresenceDetail = JsonConfig.settings.discordUsername;
            presenceManager.discordPresenceState = "is away from keyboard for ";
            presenceManager.discordLargeImageKey = this.TextBox_largeImageKey.Text;
            presenceManager.discordLargeImageText = this.TextBox_largeImageText.Text;
            presenceManager.discordSmallImageKey = this.TextBox_smallImageKey.Text;
            presenceManager.discordSmallImageText = this.TextBox_smallImageText.Text;
            presenceManager.UpdatePresence();
            Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
        }
    }

}
