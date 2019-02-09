using Chat.Configurations;
using Chat.Configurations.Models;
using Chat.Updater.ArgumentTool;
using Chat.Updater.ViewModels;
using MahApps.Metro;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Chat.Updater
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppOnStartup(object sender, StartupEventArgs e)
        {
            var updaterArguments = new UpdaterArguments(e.Args);
            var updaterViewModel = new UpdaterViewModel(updaterArguments);

            var window = new MainWindow
            {
                DataContext = updaterViewModel, WindowStartupLocation = WindowStartupLocation.CenterScreen
            };

            var colorConfigurationController = new ConfigurationController<ColorConfiguration>();
            var colorConfiguration = colorConfigurationController.ReadConfiguration(new ColorConfiguration
            {
                Color = "Steel"
            });

            ThemeManager.AddAccent("Orgadata", new Uri("pack://application:,,,/Chat.Updater;component/Styles/OrgadataTheme.xaml"));

            var foundAccent = ThemeManager.Accents.FirstOrDefault(accent =>
                accent.Name.Equals(colorConfiguration.Color, StringComparison.CurrentCultureIgnoreCase));
            var theme = ThemeManager.DetectAppStyle(Current);
            ThemeManager.ChangeAppStyle(Current, foundAccent, theme.Item1);

            window.Show();

            Task.Run(() => { updaterViewModel.Update(); });
        }
    }
}
