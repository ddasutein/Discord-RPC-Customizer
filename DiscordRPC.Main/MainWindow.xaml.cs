using System;
using System.Windows;
using System.Diagnostics;
using NativeHelpers;
using System.Reflection;
using System.Threading;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Shell;

namespace DiscordRPC.Main
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// https://github.com/discordapp/discord-rpc/blob/master/examples/button-clicker/Assets/DiscordController.cs
    /// </summary>
    /// 

    public partial class MainWindow : PerMonitorDPIWindow 
    {

        // Debug only
        private string TAG = "MainWindow.xaml: ";

        // Global Variables


        // DiscordRPC.Core Library
        public DiscordRpcClient client;

        // Classes
        GetDiscordProcess getDiscordProcess = new GetDiscordProcess();
        GetSpotifyProcess getSpotifyProcess = new GetSpotifyProcess();
        JumpListManager jumpListManager = new JumpListManager();

        // Threads
        Thread getSpotifyThread;

        public MainWindow()
        {

            InitializeComponent();
            LoadUserSettings();
            jumpListManager.LoadJumpLists();

            // Start threads
            getSpotifyThread = new Thread(new ThreadStart(getSpotifyProcess.SpotifyProcess));
            getSpotifyThread.IsBackground = true;     
            getSpotifyThread.Start();

            // Run process check
            getDiscordProcess.DiscordProcessName();
            textBlockDiscordBuildType.Text = getDiscordProcess.DiscordBuildInfo.ToString();

            // Show application version in Settings
            var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            textBlockVersionNumber.Text = "Version: " + version.Remove(version.Length - 2);

            if (getSpotifyProcess.IsSpotifyOpened == true)
            {
                MessageBox.Show("DiscordRPC has detected Spotify is running. Your rich presence or Spotify presence will not update until you shutdown this client.", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                StartDiscordPresence();
            }

        }

        public bool isDiscordOpen(string name)
        {
            foreach (Process clsProcess in Process.GetProcesses())
            {
                if (clsProcess.ProcessName.Contains(name))
                {
                    return true;
                }
            }
            return false;
        }

        private void StartDiscordPresence()
        {

            Debug.WriteLine(TAG + "Starting Discord Presence");
            client = new DiscordRpcClient(Properties.Settings.Default.discord_client_id);

            this.Button_Initialize_Discord.IsEnabled = false;
            this.Button_RunCallbacks.IsEnabled = true;
            this.Button_Update.IsEnabled = true;
            this.Button_Shutdown.IsEnabled = true;

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Debug.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Debug.WriteLine("Received Update! {0}", e.Presence);
            };

            //Connect to the RPC
            Debug.WriteLine(TAG + "Connecting to Discord Rich Presence");
            client.Initialize();

            Debug.WriteLine(TAG + "Discord RPC is online");
            this.SetStatusBarMessage("Discord RPC is online");

            Debug.WriteLine(TAG + "Setting client rich presence");
            //Set the rich presence
            client.SetPresence(new RichPresence()
            {
                Details = Properties.Settings.Default.discord_details_status,
                State = Properties.Settings.Default.discord_status_status,
                
                Assets = new Assets()
                {
                    LargeImageKey = Properties.Settings.Default.discord_largeImageKey,
                    LargeImageText = Properties.Settings.Default.discord_largeImageText,
                    SmallImageKey = Properties.Settings.Default.discord_smallImageKey,
                    SmallImageText = Properties.Settings.Default.discord_smallImageText,
                }
            });

            statusIconImage.Source = new BitmapImage(new Uri("Resources/icons8_online.png", UriKind.Relative));
        }

        private void LoadUserSettings()
        {

            // If application version is updated, move user settings to new version
            if (Properties.Settings.Default.UpgradeRequired)
            {
                Debug.WriteLine(TAG + "User settings upgraded");
                Properties.Settings.Default.Upgrade();
                Properties.Settings.Default.UpgradeRequired = false;
                Properties.Settings.Default.Save();
            }

            this.TextBox_clientId.Text = Properties.Settings.Default.discord_client_id;
            this.TextBox_state.Text = Properties.Settings.Default.discord_status_status;
            this.TextBox_details.Text = Properties.Settings.Default.discord_details_status;
            this.TextBox_startTimestamp.Text = Properties.Settings.Default.discord_startTimeStamp;
            this.TextBox_endTimestamp.Text = Properties.Settings.Default.discord_endTimeStamp;
            this.TextBox_largeImageKey.Text = Properties.Settings.Default.discord_largeImageKey;
            this.TextBox_largeImageText.Text = Properties.Settings.Default.discord_largeImageText;
            this.TextBox_smallImageKey.Text = Properties.Settings.Default.discord_smallImageKey;
            this.TextBox_smallImageText.Text = Properties.Settings.Default.discord_smallImageText;
            this.TextBox_startTimestamp.Text = this.DateTimeToTimestamp(DateTime.UtcNow).ToString();
            //this.TextBox_endTimestamp.Text = this.DateTimeToTimestamp(DateTime.UtcNow.AddHours(1)).ToString();

            if (TextBox_clientId.Text.Length == 0)
            {
                MessageBox.Show("Client ID is empty. Please enter your 'Client ID' in Settings", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.Button_Initialize_Discord.IsEnabled = true;
            }
            else
            {
                StartDiscordPresence();
            }

        }

        /// <summary>
        /// Update the presence status from what's in the UI fields.
        /// </summary>
        /// 

        private bool isTimeStampEnabled = false;

        private void updatePresence()
        {
            // Debug only
            Debug.WriteLine(TAG + "Details: " + TextBox_details.Text);
            Debug.WriteLine(TAG + "State: " + TextBox_state.Text);
            Debug.WriteLine(TAG + "LargeImageKey: " + TextBox_largeImageKey.Text);
            Debug.WriteLine(TAG + "LargeImageText: " + TextBox_largeImageText.Text);
            Debug.WriteLine(TAG + "SmallImageKey: " + TextBox_smallImageKey.Text);
            Debug.WriteLine(TAG + "SmallImageText: " + TextBox_smallImageText.Text);
            Debug.WriteLine(TAG + "Updated presence and settings");

            // Show MessageBox to notify user Spotify client is running
            if (getSpotifyProcess.IsSpotifyOpened == true)
            {
                MessageBox.Show("DiscordRPC has detected Spotify is running. Your rich presence or Spotify presence will not update until you shutdown this client.", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            if (isTimeStampEnabled == true)
            {
                client.SetPresence(new RichPresence()
                {
                    Details = this.TextBox_details.Text,
                    State = this.TextBox_state.Text,
                    Timestamps = Timestamps.Now,

                    Assets = new Assets()
                    {
                        LargeImageKey = this.TextBox_largeImageKey.Text,
                        LargeImageText = this.TextBox_largeImageText.Text,
                        SmallImageKey = this.TextBox_smallImageKey.Text,
                        SmallImageText = this.TextBox_smallImageText.Text,
                    }
                });

            }
            else if (isTimeStampEnabled == false)
            {
                client.SetPresence(new RichPresence()
                {
                    Details = this.TextBox_details.Text,
                    State = this.TextBox_state.Text,
                    Timestamps = null,

                    Assets = new Assets()
                    {
                        LargeImageKey = this.TextBox_largeImageKey.Text,
                        LargeImageText = this.TextBox_largeImageText.Text,
                        SmallImageKey = this.TextBox_smallImageKey.Text,
                        SmallImageText = this.TextBox_smallImageText.Text,
                    }
                });
            }

            saveAllSettings();

        }

        /// <summary>
        /// Calls ReadyCallback(), DisconnectedCallback(), ErrorCallback().
        /// </summary>
        private void RunCallbacks()
        {
            //DiscordRpc.RunCallbacks();

            this.SetStatusBarMessage("Callbacks run.");
        }

        /// <summary>
        /// Stop RPC.
        /// </summary>
        private void Shutdown()
        {
            //DiscordRpc.Shutdown();

            if (MessageBox.Show("Are you sure you want to shutdown Discord RPC?", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                client.Dispose();
                this.SetStatusBarMessage("Discord RPC is offline.");
                this.Button_RunCallbacks.IsEnabled = false;
                this.Button_Update.IsEnabled = false;
                this.Button_Shutdown.IsEnabled = false;
                this.Button_Initialize_Discord.IsEnabled = true;
                statusIconImage.Source = new BitmapImage(new Uri("Resources/icons8_offline.png", UriKind.Relative));
            }
            else
            {
                // Do nothing
                return;
            }

        }

        /// <summary>
        /// Called after RunCallbacks() when ready.
        /// </summary>
        private void ReadyCallback()
        {
            this.SetStatusBarMessage("Ready.");
        }

        /// <summary>
        /// Called after RunCallbacks() in cause of disconnection.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        private void DisconnectedCallback(int errorCode, string message)
        {
            this.SetStatusBarMessage(string.Format("Disconnect {0}: {1}", errorCode, message));
        }

        /// <summary>
        /// Called after RunCallbacks() in cause of error.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        private void ErrorCallback(int errorCode, string message)
        {
            this.SetStatusBarMessage(string.Format("Error {0}: {1}", errorCode, message));
        }

        /// <summary>
        /// Just set a message to be displayed in the status bar at the window's bottom.
        /// </summary>
        /// <param name="message"></param>
        private void SetStatusBarMessage(string message)
        {
            this.Label_Status.Content = message;
        }

        /// <summary>
        /// Convert a DateTime object into a timestamp.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private long DateTimeToTimestamp(DateTime dt)
        {
            return (dt.Ticks - 621355968000000000) / 10000000;
        }

        /*
		=============================================
		Event
		=============================================
		*/

        /// <summary>
        /// Called by clicking on the "Initialize" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        String clientID;

        public void startApp()
        {
            clientID = this.TextBox_clientId.Text;
            bool isNumeric = ulong.TryParse(clientID, out ulong n);

            if (!isNumeric)
            {
                MessageBox.Show("The client ID is either empty or not a numeric value.", "Client ID Error");

                return;
            }

            //this.Initialize(clientID);

            Properties.Settings.Default.discord_client_id = this.TextBox_clientId.Text;
            Properties.Settings.Default.Save();
        }

        private void Button_Initialize_Click(object sender, RoutedEventArgs e)
        {

            if (TextBox_clientId.Text.Length == 0)
            {
                MessageBox.Show("Client ID is empty. Please enter your 'Client ID'.", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            else
            {
                StartDiscordPresence();
            }


        }

        private void saveAllSettings()
        {
            Properties.Settings.Default.discord_client_id = this.TextBox_clientId.Text;
            Properties.Settings.Default.discord_status_status = this.TextBox_state.Text;
            Properties.Settings.Default.discord_details_status = this.TextBox_details.Text;
            Properties.Settings.Default.discord_startTimeStamp = this.TextBox_startTimestamp.Text;
            Properties.Settings.Default.discord_endTimeStamp = this.TextBox_endTimestamp.Text;
            Properties.Settings.Default.discord_largeImageKey = this.TextBox_largeImageKey.Text;
            Properties.Settings.Default.discord_largeImageText = this.TextBox_largeImageText.Text;
            Properties.Settings.Default.discord_smallImageKey = this.TextBox_smallImageKey.Text;
            Properties.Settings.Default.discord_smallImageText = this.TextBox_smallImageText.Text;
            Properties.Settings.Default.Save();
        }

        private void Button_save_settings_Click(object sender, RoutedEventArgs e)
        {
            saveAllSettings();
        }


        private void OnChecked(object sender, RoutedEventArgs e)
        {
            bool chkbox_status = true;
            Properties.Settings.Default.run_at_startup = chkbox_status;
            Properties.Settings.Default.Save();
        }

        private void OnUnChecked(object sender, RoutedEventArgs e)
        {
            bool chkbox_status = false;
            Properties.Settings.Default.run_at_startup = chkbox_status;
        }

        /// <summary>
        /// Called by clicking on the "Update" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 

        private bool isButtonUpdateClicked = false;

        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            updatePresence();
        }



        /// <summary>
        /// Called by clicking on the "RunCallbacks" button. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_RunCallbacks_Click(object sender, RoutedEventArgs e)
        {
            this.RunCallbacks();
        }

        /// <summary>
        /// Called by clicking on the "Shutdown" button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Shutdown_Click(object sender, RoutedEventArgs e)
        {
            Shutdown();
        }

        private void Button_open_discord_api(object sender, RoutedEventArgs e)
        {
            // Hyperlink to Discord API page
            System.Diagnostics.Process.Start("https://discordapp.com/developers/applications/me");
        }

        private void Button_reset_app(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show("Do you want to reset this application? Application will close after a reset.", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ResetApplication resetApplication = new ResetApplication();
                resetApplication.deleteDirectory();
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
    }

}
