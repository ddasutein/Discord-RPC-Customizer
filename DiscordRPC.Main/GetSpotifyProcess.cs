using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
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
                        // Do nothing
                    }

                    Thread.Sleep(1000);
                }
            }
            catch (ThreadAbortException ate)
            {
                Debug.WriteLine(TAG + ate.ToString());
            }
        }


    }
}
