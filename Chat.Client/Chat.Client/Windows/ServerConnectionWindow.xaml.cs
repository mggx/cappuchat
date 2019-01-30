using System;
using System.Windows;

namespace Chat.Client.Windows
{
    /// <summary>
    /// Interaction logic for ServerConnectionWindow.xaml
    /// </summary>
    public partial class ServerConnectionWindow
    {
        public Models.Config Config;
        public ServerConnectionWindow()
        {
            InitializeComponent();
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            TxtBoxPort.Text = Config.Port;
            TxtBoxHost.Text = Config.Host;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Config = new Models.Config();
            Config.Host = TxtBoxHost.Text;
            Config.Port = TxtBoxPort.Text;
            DialogResult = true;
            Close();
        }

        private void BtnAbort_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
