using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
                MessageBox.Show(ex.ToString(), Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title);
                Debug.WriteLine(ex.ToString());
            }

        }

        public void deleteConfig()
        {
            try
            {
                File.Delete("pref.json");
                Application.Current.Shutdown();
                Debug.WriteLine("Application has shutdown");
            }
            catch (Exception e)
            {

            }
        }

    }
}

