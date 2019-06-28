using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows;

namespace DiscordRPC.Main
{
    class SpotifyProcessListener
    {

        // Debug only
        private readonly string TAG = "GetSpotifyProcess: ";

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
#if DEBUG
                        Debug.WriteLine(TAG + "Spotify process detected");
#endif
                        isSpotifyOpened = true;
                        
                    }
                    else if (spotifyProcess != null)
                    {
#if DEBUG
                        Debug.WriteLine(TAG + "Spotify process closed");
#endif
                        isSpotifyOpened = false;
                    }

                    Thread.Sleep(1500);
                }

            }
            catch (ThreadAbortException e)
            {
                MessageBox.Show(e.ToString(), Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title);
#if DEBUG
                Debug.WriteLine(TAG + e.ToString());
#endif
            }
        }


    }
}
