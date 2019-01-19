using System.Windows;
using System.Windows.Controls;

namespace ChatComponents
{
    public class ChatListViewItem : ListViewItem
    {
        static ChatListViewItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChatListViewItem), new FrameworkPropertyMetadata(typeof(ChatListViewItem)));
        }
    }
}
