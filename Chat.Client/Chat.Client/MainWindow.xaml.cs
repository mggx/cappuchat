using System.Windows;
using Chat.Client.Windows;

namespace Chat.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private SettingsWindow _settingsWindow;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void EditConnectionSettingsClick(object sender, RoutedEventArgs e)
        {
            _settingsWindow = new SettingsWindow
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };
            _settingsWindow.ShowDialog();
        }
    }
}