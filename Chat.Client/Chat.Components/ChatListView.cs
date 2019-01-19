using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ChatComponents
{
    public class ChatListView : ListView
    {
        public static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register(
            "ItemClickCommand", typeof(ICommand), typeof(ChatListView), new PropertyMetadata(default(ICommand)));

        public ICommand ItemClickCommand
        {
            get { return (ICommand) GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        static ChatListView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChatListView), new FrameworkPropertyMetadata(typeof(ChatListView)));
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ChatListViewItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ChatListViewItem();
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);

            object parameter = null;
            if (e.AddedItems.Count > 0)
                parameter = e.AddedItems[0];

            ItemClickCommand?.Execute(parameter);
        }

        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnItemsChanged(e);

            var indexOfLastItem = Items.Count - 1;
            if (indexOfLastItem > -1)
                ScrollIntoView(Items[indexOfLastItem]);
        }
    }
}
