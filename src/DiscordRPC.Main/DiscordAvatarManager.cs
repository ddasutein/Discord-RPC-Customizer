using System.Diagnostics;

namespace DiscordRPC.Main
{
    public class DiscordAvatarManager
    {
        public void DiscordAvatar128()
        {
            Process.Start(JsonConfig.settings.discordAvatarSmall);
        }

        public void DiscordAvatar256()
        {
            Process.Start(JsonConfig.settings.discordAvatarMedium);
        }

        public void DiscordAvatar1024()
        {
            Process.Start(JsonConfig.settings.discordAvatarLarge);
        }
    }
}
