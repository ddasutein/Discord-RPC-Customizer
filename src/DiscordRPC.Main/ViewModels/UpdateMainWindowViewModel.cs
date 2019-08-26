using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DiscordRPC.Message;

namespace DiscordRPC.Main.ViewModels
{
    public class UpdateMainWindowViewModel
    {
        // ViewModel
        static MainViewModel mainViewModel = new MainViewModel();

        public static void UpdateDiscordRPCConnectionStatus(string status)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Window mainWindow = Application.Current.MainWindow;
                mainWindow.DataContext = mainViewModel;
            });
            mainViewModel.discordConnectionStatusViewModel.Status = status;
        }

        public static void UpdateUsernameData(string username)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Window mainWindow = Application.Current.MainWindow;
                mainWindow.DataContext = mainViewModel;
            });
            mainViewModel.discordProfileInfoViewModel.DiscordUsername = username;
        }

        public static void UpdateUserAvatarData(string url)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Window mainWindow = Application.Current.MainWindow;
                mainWindow.DataContext = mainViewModel;
            });
            mainViewModel.discordProfileInfoViewModel.DiscordAvatarUri = url;
        }

        public static void UpdateUserStatus(string status)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Window mainWindow = Application.Current.MainWindow;
                mainWindow.DataContext = mainViewModel;
            });
            mainViewModel.userStatusChangeViewModel.Status = status;
        }
    }
}
