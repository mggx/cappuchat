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

#pragma warning disable
        private void ChatViewOnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
#pragma warning restore
        {
            ChatView.BroadcastListView.LazyScrollToBottom = true;
        }
    }
}
