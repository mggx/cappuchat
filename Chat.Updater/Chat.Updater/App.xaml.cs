using Chat.Configurations;
using Chat.Configurations.Models;
using Chat.Updater.ArgumentTool;
using Chat.Updater.ViewModels;
using MahApps.Metro;
using System;
using System.Diagnostics;
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
            foreach (var proc in Process.GetProcessesByName("Chat.Client"))
            {
                proc.Kill();
            }

            var serverConfigurationController = new ConfigurationController<ServerConfiguration>();
            if (!serverConfigurationController.TryReadConfiguration(out var serverConfiguration))
            {
                MessageBox.Show("Could not retrieve Configuration to download files.");
            }

            var chatClientPath = $"{Environment.CurrentDirectory}\\Chat.Client.exe";

            string[] arguments =
            {
                $"-assemblyPath=\"{chatClientPath}\" ",
                $"-host=\"{serverConfiguration.Host}\" ",
                $"-ftpuser=\"{serverConfiguration.FtpUser}\" ",
                $"-ftppassword=\"{serverConfiguration.FtpPassword}\" "
            };

            var updaterArguments = new UpdaterArguments(arguments);
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

            Task.Run(async () =>
            {
                await updaterViewModel.Update(); 
            });
        }
    }
}
