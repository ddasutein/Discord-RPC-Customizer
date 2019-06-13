using DiscordRPC.Main.ViewModels;

namespace DiscordRPC.Main
{
    public class MainViewModel
    {
        public DiscordConnectionStatusViewModel discordConnectionStatusViewModel { get;  set; }
        public DiscordProfileInfoViewModel discordProfileInfoViewModel { get; set; }

        public MainViewModel()
        {
            discordConnectionStatusViewModel = new DiscordConnectionStatusViewModel();
            discordProfileInfoViewModel = new DiscordProfileInfoViewModel();
        }
    }

}
