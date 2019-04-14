using System.Windows;
using System.Windows.Controls;

namespace ChatComponents
{
    public class ChatListViewItem : ListViewItem
    {
        public static readonly DependencyProperty OwnMessageProperty = DependencyProperty.Register(
            "OwnMessage", typeof(bool), typeof(ChatListViewItem), new PropertyMetadata(default(bool)));

        public bool OwnMessage
        {
            get { return (bool) GetValue(OwnMessageProperty); }
            set { SetValue(OwnMessageProperty, value); }
        }

        static ChatListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChatListViewItem), new FrameworkPropertyMetadata(typeof(ChatListViewItem)));
        }
    }
}
