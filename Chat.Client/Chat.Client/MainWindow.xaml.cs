using System.Windows;
using Chat.Client.Configuration;
using Chat.Client.Windows;
using MahApps.Metro.Controls.Dialogs;

namespace Chat.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private AccentThemeWindow _accentThemeWindow;
        private ServerConnectionWindow _serverConnectionWindow;

        private IConfigController _configController;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ChangeAppStyleButtonClick(object sender, RoutedEventArgs e)
        {
            if (_accentThemeWindow != null)
            {
                _accentThemeWindow.Activate();
                return;
            }

            _accentThemeWindow = new AccentThemeWindow
            {
                Owner = this
            };

            _accentThemeWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            _accentThemeWindow.Closed += (o, args) => _accentThemeWindow = null;
            _accentThemeWindow.Show();
        }

        private void EditServerConnectionButtonClick(object sender, RoutedEventArgs e)
        {
            _configController = new ConfigController();
            _serverConnectionWindow = new ServerConnectionWindow
            {
                Owner = this
            };

            _serverConnectionWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            Models.Config config = _configController.ReadConfig();

            _serverConnectionWindow.Config = config;

            if (_serverConnectionWindow.ShowDialog() == true)
            {
                _configController.WriteConfig(_serverConnectionWindow.Config);
                _serverConnectionWindow = null;
                Dispatcher.Invoke(async () =>
                {
                    await this.ShowMessageAsync("Neustart", "Du musst den Client neustarten, damit die Änderungen wirksam werden.");
                });
            }
            else
            {
                _serverConnectionWindow = null;
            }
        }
    }
}
