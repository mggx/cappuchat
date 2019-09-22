using MahApps.Metro.Controls;
using System.Windows;

namespace Chat.Client.Dialogs
{
    /// <summary>
    /// Interaction logic for OkCancelDialogWindow.xaml
    /// </summary>
    public partial class OkCancelDialogWindow : MetroWindow
    {
        public OkCancelDialogWindow()
        {
            InitializeComponent();
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
