namespace DiscordRPC.Main
{
    public interface IDiscordPresence
    {
        string discordClientId { get; set; }
        string discordPresenceState { get; set; }
        string discordPresenceDetail { get; set; }
        string discordLargeImageKey { get; set; }
        string discordLargeImageText { get; set; }
        string discordSmallImageKey { get; set; }
        string discordSmallImageText { get; set; }
        bool useTimeStamp { get; set; }
    }
}
