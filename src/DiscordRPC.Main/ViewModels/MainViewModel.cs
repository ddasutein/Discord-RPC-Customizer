using DiscordRPC.Main.ViewModels;

namespace DiscordRPC.Main
{
    /// <summary>
    /// View Model for MainWindow.XAML only
    /// </summary>

    public class MainViewModel
    {
        public DiscordConnectionStatusViewModel discordConnectionStatusViewModel { get;  set; }
        public DiscordProfileInfoViewModel discordProfileInfoViewModel { get; set; }
        public UserStatusChangeViewModel userStatusChangeViewModel { get; set; }

        public MainViewModel()
        {
            discordConnectionStatusViewModel = new DiscordConnectionStatusViewModel();
            discordProfileInfoViewModel = new DiscordProfileInfoViewModel();
            userStatusChangeViewModel = new UserStatusChangeViewModel();
        }
    }

}
