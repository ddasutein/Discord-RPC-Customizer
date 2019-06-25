using System.Diagnostics;

namespace DiscordRPC.Main
{
    class DiscordProcessListener
    {
        private readonly string TAG = "DiscordProcessListener.cs: ";
        public bool IsDiscordRunning { get; private set; } = false;

        public string DiscordBuildInfo { get; set; }

        public void DiscordProcessName()
        {
            Process[] localAll = Process.GetProcesses();
            foreach (Process process in localAll)
            {
#if DEBUG
                Debug.WriteLine(TAG + process);
#endif

                switch (process.ProcessName)
                {
                    case "Discord":
                        DiscordBuildInfo = "Stable";
                        IsDiscordRunning = true;
                        break;
                    case "DiscordPTB":
                        DiscordBuildInfo = "Public Test Beta (PTB)";
                        IsDiscordRunning = true;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
