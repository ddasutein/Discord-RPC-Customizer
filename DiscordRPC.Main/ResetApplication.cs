using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DiscordRPC.Main
{
    class ResetApplication
    {

        public void deleteDirectory()
        {
            // Delete 'Dasutein' folder in Local directory
            string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string FolderToDelete = Path.Combine(AppDataFolder, "Dasutein");

            try
            {
                Directory.Delete(FolderToDelete, true);
                Debug.WriteLine("Deleted folder: " + AppDataFolder);

                Application.Current.Shutdown();
                Debug.WriteLine("Application has shutdown");

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }

    }
}

