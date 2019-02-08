using System.Windows;
using Chat.Updater.ViewModels;

namespace Chat.Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppOnStartup(object sender, StartupEventArgs e)
        {
            var updaterViewModel = new UpdaterViewModel(e.Args[0]);
            var window = new MainWindow { DataContext = updaterViewModel };

            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }
    }
}
