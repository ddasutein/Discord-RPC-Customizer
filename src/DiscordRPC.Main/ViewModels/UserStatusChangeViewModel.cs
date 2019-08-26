using System;
using System.Windows;

namespace DiscordRPC.Main.ViewModels
{
    public class UserStatusChangeViewModel : UserStatusChangeObserver
    {
        private string _Status = string.Empty;
        public string Status
        {
            get
            {
                if (string.IsNullOrEmpty(_Status))
                {
                    return (string)Application.Current.FindResource("mw_label_status_placeholder");
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
