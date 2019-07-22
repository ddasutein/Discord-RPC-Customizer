using DiscordRPC.Message;
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;

namespace DiscordRPC.Main
{
    public class PresenceManager : IDiscordPresence
    {
        public string discordClientId { get; set; }
        public string discordPresenceState { get; set; }
        public string discordPresenceDetail { get; set; }
        public string discordLargeImageKey { get; set; }
        public string discordLargeImageText { get; set; }
        public string discordSmallImageKey { get; set; }
        public string discordSmallImageText { get; set; }
        public bool useTimeStamp { get; set; }

        // Debug only
        private readonly string TAG = "PresenceManager: ";

        // DiscordRichPresence Library
        static DiscordRpcClient client;

        // ViewModel
        MainViewModel mainViewModel = new MainViewModel();

        /// <summary> StartTimeOutTimer method starts alongside Presence initialization
        /// Timer will run for 10 seconds and stop when it establishes a connection to Discord.
        /// If it fails to make a connection within 10 seconds, it will prompt a MessageBox dialog and loop until
        /// a connection is re-established. Note that this method is different from the 
        /// "OnConnectionFailed" event, which only checks the Discord client. 
        /// </summary>
        private static Timer TimeOutTimer;
        private void StartTimeOutTimer()
        {
            int timeout_value = 10000; // Every 10 seconds

            TimeOutTimer = new Timer(timeout_value);
            TimeOutTimer.Elapsed += OnTimedEvent;
            TimeOutTimer.AutoReset = true;
            TimeOutTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            TimeOutTimer.Stop();
            MessageBoxResult result = MessageBox.Show("Failed to connect to Discord. No internet connection. Please check your connection and try again.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            
            if (result == MessageBoxResult.OK)
            {
                TimeOutTimer.Start();
            }

        }

        public void InitializeDiscordRPC(string ClientID)
        {
#if DEBUG
            Debug.WriteLine(TAG + "Starting Discord Presence");
#else
#endif

            Application.Current.Dispatcher.Invoke(delegate
            {
                Window mainWindow = Application.Current.MainWindow;
                mainWindow.DataContext = mainViewModel;
            });
            mainViewModel.discordConnectionStatusViewModel.Status = "Starting Discord Presence....";

            StartTimeOutTimer();
            client = new DiscordRpcClient(ClientID);
            client.Initialize();
            client.OnReady += OnClientReady;
            client.OnConnectionFailed += OnConnectionFailed;
            client.OnConnectionEstablished += OnConnectionEstablished;
        }
        private void OnClientReady(object sender, ReadyMessage args)
        {
            TimeOutTimer.Stop();
            TimeOutTimer.Dispose();
#if DEBUG
            Debug.WriteLine("Received Ready from user {0}", args.User);
#else
#endif
            JsonConfig.settings.discordUsername = args.User.ToString();
            JsonConfig.settings.discordAvatarSmall = args.User.GetAvatarURL(User.AvatarFormat.PNG, User.AvatarSize.x128);
            JsonConfig.settings.discordAvatarMedium = args.User.GetAvatarURL(User.AvatarFormat.PNG, User.AvatarSize.x512);
            JsonConfig.settings.discordAvatarLarge = args.User.GetAvatarURL(User.AvatarFormat.PNG, User.AvatarSize.x1024);
            JsonConfig.SaveJson();
            mainViewModel.discordConnectionStatusViewModel.Status = "RikoRPC is online";
            mainViewModel.discordProfileInfoViewModel.DiscordUsername = args.User.ToString();
            mainViewModel.discordProfileInfoViewModel.DiscordAvatarUri = args.User.GetAvatarURL(User.AvatarFormat.PNG, User.AvatarSize.x128);

        }
        private void OnConnectionFailed(object sender, ConnectionFailedMessage args)
        {
            MessageBox.Show("Connection to Discord has failed. Check if your Discord client is running.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            mainViewModel.discordConnectionStatusViewModel.Status = "Failed to establish connection.";
        }
        private void OnConnectionEstablished(object sender, ConnectionEstablishedMessage args)
        {
            client.SetPresence(new RichPresence()
            {
                Details = JsonConfig.settings.discordPresenceDetail,
                State = JsonConfig.settings.discordPresenceState,

                Assets = new Assets()
                {
                    LargeImageKey = JsonConfig.settings.discordLargeImageKey,
                    LargeImageText = JsonConfig.settings.discordLargeImageText,
                    SmallImageKey = JsonConfig.settings.discordSmallImageKey,
                    SmallImageText = JsonConfig.settings.discordSmallImageText,
                }
            });
            client.Invoke();
        }

        public void UpdatePresence()
        {

            if (!useTimeStamp)
            {
                client.SetPresence(new RichPresence()
                {
                    Details = discordPresenceDetail,
                    State = discordPresenceState,
                    Timestamps = null,

                    Assets = new Assets()
                    {
                        LargeImageKey = discordLargeImageKey,
                        LargeImageText = discordLargeImageText,
                        SmallImageKey = discordSmallImageKey,
                        SmallImageText = discordSmallImageText
                    }
                });
                client.Invoke();
            }
            else
            {
                client.SetPresence(new RichPresence()
                {
                    Details = discordPresenceDetail,
                    State = discordPresenceState,
                    Timestamps = Timestamps.Now,

                    Assets = new Assets()
                    {
                        LargeImageKey = discordLargeImageKey,
                        LargeImageText = discordLargeImageText,
                        SmallImageKey = discordSmallImageKey,
                        SmallImageText = discordSmallImageText
                    }
                });
                client.Invoke();
            }
        }

        public void ShutdownPresence()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Window mainWindow = Application.Current.MainWindow;
                mainWindow.DataContext = mainViewModel;
                mainViewModel.discordConnectionStatusViewModel.Status = string.Empty;
                mainViewModel.discordProfileInfoViewModel.DiscordUsername = string.Empty;
                mainViewModel.discordProfileInfoViewModel.DiscordAvatarUri = string.Empty;
            });

            client.Dispose();
            discordPresenceDetail = string.Empty;
            discordPresenceState = string.Empty;
            discordLargeImageKey = string.Empty;
            discordLargeImageText = string.Empty;
            discordSmallImageKey = string.Empty;
            discordSmallImageText = string.Empty;
        }
    }

}
