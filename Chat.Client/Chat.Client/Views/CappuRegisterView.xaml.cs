using System.Windows.Controls;

namespace Chat.Client.Views
{
    /// <summary>
    /// Interaction logic for CappuRegisterView.xaml
    /// </summary>
    public partial class CappuRegisterView : UserControl
    {
        public CappuRegisterView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            ShortNameTextBox.Focus();
        }
    }
}
