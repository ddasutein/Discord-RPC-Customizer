using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace DiscordRPC.Main
{
    class GetDiscordProcess
    {
        private string discordBuildInfo;
        private bool isDiscordRunning = false;

        public bool IsDiscordRunning
        {
            get { return isDiscordRunning; }
        }

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
                        Debug.WriteLine("User is running Discord PTB build");
                        discordBuildInfo = "Public Test Beta (PTB)";
                        isDiscordRunning = true;
                    }
                    else
                    {
                        Debug.WriteLine("User is running Discord STABLE build");
                        discordBuildInfo = "Stable";
                        isDiscordRunning = true;
                    }

                }
                else
                {
                    isDiscordRunning = false;
                }

            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title);
                Debug.WriteLine(exception.ToString());
            }
        }
    }
}
