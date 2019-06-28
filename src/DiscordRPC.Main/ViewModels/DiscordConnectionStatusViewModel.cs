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
                    return "RikoRPC is offline. Go to Settings and Click 'Go online'";
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
