﻿using System;
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
        private readonly static string TAG = "MainWindow.xaml: ";

        // Global Variables
        private bool isDiscordPresenceRunning = false;
        private bool isTimeStampEnabled = false;

        // Classes
        SpotifyProcessListener getSpotifyProcess = new SpotifyProcessListener();
        JumpListManager jumpListManager = new JumpListManager();
        PresenceManager presenceManager = new PresenceManager();
        HashManager hashManager = new HashManager();

        // Threads
        Thread spotifyProcessScanThread;
        Thread StartHashManagerThread;

        public MainWindow()
        {
            InitializeComponent();
            this.Closed += ExitApplication;
            jumpListManager.LoadJumpLists();
            LoadUserStatePresence();
            CheckForUpdates();
            StartThreads();
            StartDiscordPresence();
        }

        protected void ExitApplication(object sender, EventArgs e)
        {
            // Dispose RPC when user exits application
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
        private void CheckForUpdates()
        {
            if (!AppUpdateChecker.IsUpdateAvailable)
            {
                this.Button_Download_Update.Visibility = Visibility.Hidden;
            }
            else
            {
                this.Button_Download_Update.Visibility = Visibility.Visible;
            }
        }

        private void StartThreads()
        {
            spotifyProcessScanThread = new Thread(new ThreadStart(getSpotifyProcess.SpotifyProcess));
            spotifyProcessScanThread.IsBackground = true;
            spotifyProcessScanThread.Start();
        }
        private void StartDiscordPresence()
        {

            if (String.IsNullOrEmpty(TextBox_clientId.Password))
            {
                this.Button_Update.IsEnabled = false;
                this.Button_Shutdown.IsEnabled = false;
                this.Button_Initialize_Discord.IsEnabled = true;
                statusIconImage.Source = new BitmapImage(new Uri("/RikoRPC;component/Resources/icons8_offline.png", UriKind.Relative));
                this.ImageContextItem0.IsEnabled = false;
                this.ImageContextItem1.IsEnabled = false;
                this.ImageContextItem2.IsEnabled = false;
                this.ImageContextItem2.IsEnabled = false;
                this.SetCurrentStatus("Offline");

                return;
            }
            else
            {
                hashManager.discordClientId = this.TextBox_clientId.Password;
                StartHashManagerThread = new Thread(new ThreadStart(hashManager.HashId));
                StartHashManagerThread.Start();

                this.Button_Initialize_Discord.IsEnabled = false;
                this.Button_Update.IsEnabled = true;
                this.Button_Shutdown.IsEnabled = true;
                this.Button_afk_and_lock_pc.IsEnabled = true;
                this.TextBox_clientId.IsEnabled = false;
                statusIconImage.Source = new BitmapImage(new Uri("/RikoRPC;component/Resources/icons8_online.png", UriKind.Relative));
                this.ImageContextItem0.IsEnabled = true;
                this.ImageContextItem1.IsEnabled = true;
                this.ImageContextItem2.IsEnabled = true;
                this.ImageContextItem2.IsEnabled = true;
                this.SetCurrentStatus("Online");

                // Used for checking if Discord presence is running before closing the app
                isDiscordPresenceRunning = true;
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

            this.SetCurrentStatus("Online");
            // Show MessageBox to notify user Spotify client is running
            if (getSpotifyProcess.IsSpotifyOpened)
            {
                MessageBox.Show((string)Application.Current.FindResource("app_info_spotify_process_detected"), 
                    Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, 
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
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

        private void Shutdown()
        {
            presenceManager.ShutdownPresence();
            this.Button_Update.IsEnabled = false;
            this.Button_Shutdown.IsEnabled = false;
            this.Button_Initialize_Discord.IsEnabled = true;
            this.TextBox_clientId.IsEnabled = true;
            this.Button_afk_and_lock_pc.IsEnabled = false;
            this.ImageContextItem0.IsEnabled = false;
            this.ImageContextItem1.IsEnabled = false;
            this.ImageContextItem2.IsEnabled = false;
            this.ImageContextItem3.IsEnabled = false;
            this.SetCurrentStatus("Offline");
            statusIconImage.Source = new BitmapImage(new Uri("/RikoRPC;component/Resources/icons8_offline.png", UriKind.Relative));      
    }

        String clientID;

        private void Button_Initialize_Click(object sender, RoutedEventArgs e)
        {
            clientID = this.TextBox_clientId.Password.ToString();
            bool isNumeric = ulong.TryParse(clientID, out ulong n);

            if (!isNumeric)
            {
                MessageBox.Show((string)Application.Current.FindResource("app_error_client_id_empty"),
                    Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title,
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                SaveUserStatePresence();
                StartDiscordPresence();
            }
        }

        private void SetCurrentStatus(string status)
        {
            this.currentStatusLabel.Content = status;
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
            if (MessageBox.Show((string)Application.Current.FindResource("app_info_shutdown_rpc"), 
                Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, 
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                StartHashManagerThread.Abort();
                Shutdown();
            }
            else
            {
                return;
            }
        }

        private void Button_open_discord_api(object sender, RoutedEventArgs e)
        {
            Process.Start("https://discordapp.com/developers/applications/me" + "/" + TextBox_clientId.Password);
        }

        private void Button_reset_app(object sender, RoutedEventArgs e)
        {

            if (MessageBox.Show((string)Application.Current.FindResource("app_info_reset_application"), 
                Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title,
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                ResetApplication.DeleteConfig();
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
            this.SetCurrentStatus("AFK (" + (string)Application.Current.FindResource("mw_label_status_afk") + "" + DateTime.Now + ")");
            presenceManager.useTimeStamp = true;
            presenceManager.discordPresenceDetail = JsonConfig.settings.discordUsername;
            presenceManager.discordPresenceState = (string)Application.Current.FindResource("discord_status_afk");
            presenceManager.discordLargeImageKey = this.TextBox_largeImageKey.Text;
            presenceManager.discordLargeImageText = this.TextBox_largeImageText.Text;
            presenceManager.discordSmallImageKey = this.TextBox_smallImageKey.Text;
            presenceManager.discordSmallImageText = this.TextBox_smallImageText.Text;
            presenceManager.UpdatePresence();
            //Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
        }

        private void DiscordAvatarImage_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                DiscordAvatarManager.DiscordAvatar128();
            }
            
        }

        private void ImageContextMenuItem128Click(object sender, RoutedEventArgs e)
        {
            DiscordAvatarManager.DiscordAvatar128();
        }

        private void ImageContextMenuItem256Click(object sender, RoutedEventArgs e)
        {
            DiscordAvatarManager.DiscordAvatar256();
        }

        private void ImageContextMenuItem1024Click(object sender, RoutedEventArgs e)
        {
            DiscordAvatarManager.DiscordAvatar1024();
        }

        private void Button_Download_Update_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://github.com/ddasutein/Discord-RPC-csharp/releases");
        }
    }

}
