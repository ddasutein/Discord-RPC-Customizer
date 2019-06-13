using System.ComponentModel;

namespace DiscordRPC.Main
{
    public class DiscordConnectionStatusObserve : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string status)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(status));

        }

    }
}
