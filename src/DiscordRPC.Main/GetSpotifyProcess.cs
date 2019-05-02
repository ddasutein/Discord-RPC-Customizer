using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace DiscordRPC.Main
{
    class GetSpotifyProcess
    {

        // Debug only
        private string TAG = "GetSpotifyProcess: ";

        // Global variables
        private bool isSpotifyOpened = false;

        public bool IsSpotifyOpened
        {
            get { return isSpotifyOpened; }
        }

        /// <summary>
        /// This while loop will run on a thread that has a 1 second interval to check
        /// if the Spotify.exe process is currently running.
        /// </summary>
        public void SpotifyProcess()
        {
            
            try
            {

                while (true)
                {
                    Process[] spotifyProcess = Process.GetProcessesByName("Spotify");

                    if (spotifyProcess.Length > 0)
                    {
                        Debug.WriteLine(TAG + "Spotify process detected");
                        isSpotifyOpened = true;
                        
                    }
                    else if (spotifyProcess != null)
                    {
                        Debug.WriteLine(TAG + "Spotify process closed");
                        isSpotifyOpened = false;
                    }

                    Thread.Sleep(1500);
                }

                if (!isSpotifyOpened)
                {
                    MessageBox.Show("Discord RPC has detected Spotify is running. Your rich presence or Spotify presence will not update until your RPC client or Spotify client is offline.", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
            catch (ThreadAbortException ate)
            {
                MessageBox.Show(ate.ToString(), Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title);
                Debug.WriteLine(TAG + ate.ToString());
            }
        }


    }
}
