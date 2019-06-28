using System;
using System.Windows.Media.Imaging;

namespace DiscordRPC.Main.ViewModels
{
    public class DiscordProfileInfoViewModel : DiscordConnectionStatusObserve
    {
        private string _DiscordAvatarUri = string.Empty;
        private string _DiscordUsername = string.Empty;
        public string DiscordAvatarUri
        {
            get
            {
                if (string.IsNullOrEmpty(_DiscordAvatarUri))
                {
                    var fallbackImage = new BitmapImage(new Uri("/RikoRPC;component/Resources/icons8_discord_100.png", UriKind.Relative));
                    return fallbackImage.ToString();
                }
                else
                {
                    return _DiscordAvatarUri;
                }

            }
            set
            {
                this._DiscordAvatarUri = value;
                this.OnPropertyChanged("DiscordAvatarUri");
            }
        }

        public string DiscordUsername
        {
            get
            {
                if (string.IsNullOrEmpty(_DiscordUsername))
                {
                    return "Not available";
                }
                else
                {
                    return _DiscordUsername;
                }

            }
            set
            {
                this._DiscordUsername = value;
                this.OnPropertyChanged("DiscordUsername");
            }
        }

    }
}
