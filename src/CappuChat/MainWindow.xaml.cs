using Chat.Client.Windows;
using MahApps.Metro.Controls;
using System.Windows;

namespace Chat.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
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