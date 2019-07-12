using System.Diagnostics;
using System.Threading.Tasks;

namespace DiscordRPC.Main
{
    class DiscordProcessListener
    {
        private readonly static string TAG = "DiscordProcessListener.cs: ";
        public static bool IsDiscordRunning { get; private set; } = false;

        public static string DiscordBuildType { get; set; }

        public static void DiscordProcessName()
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
                        DiscordBuildType = "Stable";
                        IsDiscordRunning = true;
                        break;
                    case "DiscordPTB":
                        DiscordBuildType = "Public Test Beta (PTB)";
                        IsDiscordRunning = true;
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
