using System.Windows;

namespace DiscordRPC.Main.ViewModels
{
    public class DiscordConnectionStatusViewModel : DiscordConnectionStatusObserve
    {
        private string _Status = string.Empty;
        public string Status
        {
            get
            {
                if (string.IsNullOrEmpty(_Status))
                {
                    return (string)Application.Current.FindResource("mw_status_offline");
                }
                else
                {
                    return _Status;
                }

            }
            set
            {
                this._Status = value;
                this.OnPropertyChanged("Status");
            }
        }
    }
}
