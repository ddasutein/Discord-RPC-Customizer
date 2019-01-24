using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiscordRPC.Main
{
    class GetDiscordProcess
    {
        private string discordBuildInfo;

        public string DiscordBuildInfo
        {
            get { return discordBuildInfo; }
        }

        public void DiscordProcessName()
        {

            // Check if Discord (stable release) process is opened or closed
            Process[] DiscordStableProcess = Process.GetProcessesByName("Discord");

            // Check if Discord (public test build) process is opened or closed
            Process[] DiscordPTBProcess = Process.GetProcessesByName("DiscordPTB");

            try
            {

                if (DiscordStableProcess.Length > 0 || DiscordPTBProcess.Length > 0)
                {

                    if (DiscordPTBProcess.Length > 0)
                    {
                        Debug.WriteLine("User is running Discord STABLE build");
                        discordBuildInfo = "Public Test Beta (PTB)";
                    }
                    else
                    {
                        Debug.WriteLine("User is running Discord PTB build");
                        discordBuildInfo = "Stable";
                    }

                }
                else
                {
                    MessageBox.Show("Discord is not running.", Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title, MessageBoxButton.OK, MessageBoxImage.Error);
                    Debug.WriteLine("Discord is not running. Shutting down application");
                    Application.Current.Shutdown();
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                Debug.WriteLine(exception.ToString());
            }
        }
    }
}
