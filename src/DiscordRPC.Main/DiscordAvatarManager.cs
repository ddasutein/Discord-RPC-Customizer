using System.Diagnostics;

namespace DiscordRPC.Main
{
    public class DiscordAvatarManager
    {
        public static void DiscordAvatar128()
        {
            Process.Start(JsonConfig.settings.discordAvatarSmall);
        }

        public static void DiscordAvatar256()
        {
            Process.Start(JsonConfig.settings.discordAvatarMedium);
        }

        public static void DiscordAvatar1024()
        {
            Process.Start(JsonConfig.settings.discordAvatarLarge);
        }
    }
}
