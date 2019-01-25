using System.Windows;
using System.Windows.Controls;

namespace Chat.Client.Views
{
    /// <summary>
    /// Interaction logic for CappuChatsView.xaml
    /// </summary>
    public partial class CappuChatsView : UserControl
    {
        public CappuChatsView()
        {
            InitializeComponent();
        }

        private void ChatViewOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ChatView.BroadcastListView.LazyScrollToBottom = true;
        }
    }
}
