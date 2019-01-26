using System.Windows;
using Chat.Client.Windows;

namespace Chat.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private AccentThemeWindow _accentThemeWindow;
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
                Left = Left + ActualWidth / 2.0, Top = Top + ActualHeight / 2.0, Owner = this
            };

            _accentThemeWindow.Closed += (o, args) => _accentThemeWindow = null;
            _accentThemeWindow.Show();
        }
    }
}
