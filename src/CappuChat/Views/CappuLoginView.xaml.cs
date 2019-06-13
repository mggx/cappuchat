namespace Chat.Client.Views
{
    /// <summary>
    /// Interaction logic for CappuLoginView.xaml
    /// </summary>
    public partial class CappuLoginView
    {
        public CappuLoginView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UsernameTextBox.Focus();
        }
    }
}
