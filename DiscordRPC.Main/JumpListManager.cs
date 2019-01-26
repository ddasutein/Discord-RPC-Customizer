using System.Windows;
using System.Windows.Shell;

namespace DiscordRPC.Main
{
    class JumpListManager
    {

        /// <summary>
        /// Adding context items to Jump List for Windows 7, 8.1, and 10.
        /// </summary>

        public void LoadJumpLists()
        {
            JumpTask GitHubJumpTask = new JumpTask
            {
                Title = "Contribute on GitHub",
                Description = "Developed by Dasutein",
                ApplicationPath = "https://github.com/ddasutein/Discord-RPC-csharp",
                IconResourceIndex = 0

            };

            JumpList jumpList = new JumpList();
            jumpList.ShowFrequentCategory = false;
            jumpList.ShowRecentCategory = false;

            // Add Jump list items here
            jumpList.JumpItems.Add(GitHubJumpTask);

            JumpList.SetJumpList(Application.Current, jumpList);
        }

    }
}
