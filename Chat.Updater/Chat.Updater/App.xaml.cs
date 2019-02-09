using System.Threading.Tasks;
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
            var arg = string.Empty;
            if (e.Args.Length == 1)
                arg = e.Args[0];

            var updaterViewModel = new UpdaterViewModel(arg);
            var window = new MainWindow
            {
                DataContext = updaterViewModel, WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            window.Show();

            Task.Run(() => { updaterViewModel.Update(); });
        }
    }
}
