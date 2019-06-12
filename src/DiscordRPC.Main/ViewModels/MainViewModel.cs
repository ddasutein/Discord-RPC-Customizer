using DiscordRPC.Main.ViewModels;

namespace DiscordRPC.Main
{
    public class MainViewModel
    {
        public DiscordConnectionStatusViewModel discordConnectionStatusViewModel { get;  set; }

        public MainViewModel()
        {
            discordConnectionStatusViewModel = new DiscordConnectionStatusViewModel();
        }
    }

}
