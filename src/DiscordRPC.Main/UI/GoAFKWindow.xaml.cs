using System;
using System.Windows;
using System.Timers;
using System.Diagnostics;
using DiscordRPC.Main.ViewModels;

namespace DiscordRPC.Main.UI
{

    public partial class GoAFKWindow : Window
    {
        public GoAFKWindow()
        {
            InitializeComponent();
            BeginAFKTimer();
        }

        private static Timer TimeOutTimer;

        private void BeginAFKTimer()
        {
            int timeout_value = 3000; 

            TimeOutTimer = new Timer(timeout_value);
            TimeOutTimer.Elapsed += OnTimedEvent;
            TimeOutTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            StopTimer();
            StartAFK();
        }

        private void StartAFK()
        {

            PresenceManager presenceManager = new PresenceManager();
            presenceManager.useTimeStamp = true;
            presenceManager.discordPresenceDetail = JsonConfig.settings.discordUsername;
            presenceManager.discordPresenceState = (string)Application.Current.FindResource("discord_status_afk");
            presenceManager.discordLargeImageKey = JsonConfig.settings.discordLargeImageKey;
            presenceManager.discordLargeImageText = JsonConfig.settings.discordLargeImageText;
            presenceManager.discordSmallImageKey = JsonConfig.settings.discordSmallImageText;
            presenceManager.discordSmallImageText = JsonConfig.settings.discordSmallImageText;
            presenceManager.UpdatePresence();

            UpdateMainWindowViewModel.UpdateUserStatus("AFK (" + (string)Application.Current.FindResource("mw_label_status_placeholder_afk") + " " + DateTime.Now + ")");
            Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
            this.Dispatcher.Invoke((Action)(() =>
            {
                this.Close();
            }));

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            StopTimer();
            this.Close();
        }

        protected void StopTimer()
        {
            TimeOutTimer.Stop();
            TimeOutTimer.Elapsed -= OnTimedEvent;
            TimeOutTimer.Dispose();
        }
    }
}
