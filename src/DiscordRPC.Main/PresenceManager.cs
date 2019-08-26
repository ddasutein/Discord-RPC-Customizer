using DiscordRPC.Main.ViewModels;
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
        public static DiscordRpcClient client;

        /// <summary> StartTimeOutTimer method starts alongside Presence initialization
        /// Timer will run for 10 seconds and stop when it establishes a connection to Discord.
        /// If it fails to make a connection within 10 seconds, it will prompt a MessageBox dialog and loop until
        /// a connection is re-established. Note that this method is different from the 
        /// "OnConnectionFailed" event, which only checks the Discord client. 
        /// </summary>
        /// 

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

            UpdateMainWindowViewModel.UpdateDiscordRPCConnectionStatus((string)Application.Current.FindResource("mw_status_start"));
            UpdateMainWindowViewModel.UpdateUserStatus((string)Application.Current.FindResource("mw_label_status_placeholder_waiting"));

            StartTimeOutTimer();
            client = new DiscordRpcClient(ClientID);
            client.Initialize();
            client.OnReady += OnClientReady;

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

            UpdateMainWindowViewModel.UpdateDiscordRPCConnectionStatus((string)Application.Current.FindResource("mw_status_online"));
            UpdateMainWindowViewModel.UpdateUserAvatarData(args.User.GetAvatarURL(User.AvatarFormat.PNG, User.AvatarSize.x128));
            UpdateMainWindowViewModel.UpdateUsernameData(args.User.ToString());
            UpdateMainWindowViewModel.UpdateUserStatus((string)Application.Current.FindResource("mw_label_status_placeholder_online"));

        }
        private void OnConnectionFailed(object sender, ConnectionFailedMessage args)
        {
            MessageBox.Show("Connection to Discord has failed. Check if your Discord client is running.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Warning);

            UpdateMainWindowViewModel.UpdateDiscordRPCConnectionStatus((string)Application.Current.FindResource("mw_status_on_connection_failed"));
            UpdateMainWindowViewModel.UpdateUserStatus((string)Application.Current.FindResource("mw_label_status_placeholder_waiting"));

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

            client.OnConnectionFailed += OnConnectionFailed;
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

            UpdateMainWindowViewModel.UpdateDiscordRPCConnectionStatus(string.Empty);
            UpdateMainWindowViewModel.UpdateUsernameData(string.Empty);
            UpdateMainWindowViewModel.UpdateUserAvatarData(string.Empty);
            UpdateMainWindowViewModel.UpdateUserStatus((string)Application.Current.FindResource("mw_label_status_placeholder"));

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
