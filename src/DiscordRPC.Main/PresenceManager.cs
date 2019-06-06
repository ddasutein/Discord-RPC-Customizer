using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace DiscordRPC.Main
{
    class PresenceManager : IDiscordPresence
    {
        public string statusBarMessage { get; set; }
        public string discordClientId { get; set; }
        public string discordPresenceState { get; set; }
        public string discordPresenceDetail { get; set; }
        public string discordLargeImageKey { get; set; }
        public string discordLargeImageText { get; set; }
        public string discordSmallImageKey { get; set; }
        public string discordSmallImageText { get; set; }
        public bool useTimeStamp { get; set; }

        // DiscordRPC.Core Library
        static DiscordRpcClient client;

        // Debug only
        static string TAG = "PresenceManager: ";

        public void InitializeDiscordRPC(string ClientID)
        {
            Debug.WriteLine(TAG + "Starting Discord Presence");
            Debug.WriteLine(TAG + "Connecting to Discord...");
            client = new DiscordRpcClient(ClientID);
            client.Initialize();

            //Subscribe to events
            client.OnReady += (sender, e) =>
            {
                Debug.WriteLine("Received Ready from user {0}", e.User.Username);
            };

            client.OnPresenceUpdate += (sender, e) =>
            {
                Debug.WriteLine("Received Update! {0}", e.Presence);
            };

            client.OnConnectionFailed += (sender, e) =>
            {
                MessageBox.Show("Connection to Discord has failed. Check if your Discord client is running.", "Connection Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            };

            client.OnConnectionEstablished += (sender, e) =>
            {
                Debug.WriteLine(TAG + "Discord RPC is online");
                Debug.WriteLine(TAG + "Setting client rich presence");

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
            };

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
            client.Dispose();
        }
    }
}
