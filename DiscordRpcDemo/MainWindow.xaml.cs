using System;
using System.Windows;
using System.Diagnostics;

namespace DiscordRpcDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// https://github.com/discordapp/discord-rpc/blob/master/examples/button-clicker/Assets/DiscordController.cs
    /// </summary>
    public partial class MainWindow : Window
    {
        private DiscordRpc.RichPresence presence;

        DiscordRpc.EventHandlers handlers;

        public MainWindow()
        {
            InitializeComponent();
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

            startApp();
            beginUpdatePresence();

            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            textBlockVersionNumber.Text = "Version: " + version;

        }

        /*
		=============================================
		Private
		=============================================
		*/

        /// <summary>
        /// Initialize the RPC.
        /// </summary>
        /// <param name="clientId"></param>
        private void Initialize(string clientId)
        {
            handlers = new DiscordRpc.EventHandlers();

            handlers.readyCallback = ReadyCallback;
            handlers.disconnectedCallback += DisconnectedCallback;
            handlers.errorCallback += ErrorCallback;

            DiscordRpc.Initialize(clientId, ref handlers, true, null);

            this.SetStatusBarMessage("Initialized.");

        }

        /// <summary>
        /// Update the presence status from what's in the UI fields.
        /// </summary>


        public void beginUpdatePresence()
        {

            string t_1 = this.TextBox_details.Text;

            presence.details = this.TextBox_details.Text;
            presence.state = this.TextBox_state.Text;

            if (long.TryParse(this.TextBox_startTimestamp.Text, out long startTimestamp))
            {
                presence.startTimestamp = startTimestamp;
            }

            if (long.TryParse(this.TextBox_endTimestamp.Text, out long endTimestamp))
            {
                presence.endTimestamp = endTimestamp;
            }

            presence.largeImageKey = this.TextBox_largeImageKey.Text;
            presence.largeImageText = this.TextBox_largeImageText.Text;
            presence.smallImageKey = this.TextBox_smallImageKey.Text;
            presence.smallImageText = this.TextBox_smallImageText.Text;
            saveAllSettings();

            DiscordRpc.UpdatePresence(ref presence);

            this.SetStatusBarMessage("Presence updated.");
        }
        private void UpdatePresence()
        {
            beginUpdatePresence();

        }

        /// <summary>
        /// Calls ReadyCallback(), DisconnectedCallback(), ErrorCallback().
        /// </summary>
        private void RunCallbacks()
        {
            DiscordRpc.RunCallbacks();

            this.SetStatusBarMessage("Callbacks run.");
        }

        /// <summary>
        /// Stop RPC.
        /// </summary>
        private void Shutdown()
        {
            DiscordRpc.Shutdown();

            this.SetStatusBarMessage("Offline.");
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

            this.Initialize(clientID);

            this.Button_Initialize_Discord.IsEnabled = false;
            this.Button_RunCallbacks.IsEnabled = true;
            this.Button_Update.IsEnabled = true;
            this.Button_Shutdown.IsEnabled = true;

            Properties.Settings.Default.discord_client_id = this.TextBox_clientId.Text;
            Properties.Settings.Default.Save();
        }

        private void Button_Initialize_Click(object sender, RoutedEventArgs e)
        {
            startApp();
            beginUpdatePresence();
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
        private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            this.UpdatePresence();
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
            this.Shutdown();

            this.Button_RunCallbacks.IsEnabled = false;
            this.Button_Update.IsEnabled = false;
            this.Button_Shutdown.IsEnabled = false;
            this.Button_Initialize_Discord.IsEnabled = true;
        }

        private void Button_open_discord_api(object sender, RoutedEventArgs e)
        {
            // Hyperlink to Discord API page
            System.Diagnostics.Process.Start("https://discordapp.com/developers/applications/me");
        }

    }

}
