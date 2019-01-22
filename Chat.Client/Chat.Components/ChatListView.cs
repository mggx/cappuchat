using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChatComponents
{
    public class ChatListView : ListView
    {
        private ScrollViewer _scrollViewer;

        public string UserMessageBooleanPath { get; set; }

        public static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register(
            "ItemClickCommand", typeof(ICommand), typeof(ChatListView), new PropertyMetadata(default(ICommand)));

        public ICommand ItemClickCommand
        {
            get { return (ICommand) GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        public ChatListView()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            Decorator border = VisualTreeHelper.GetChild(this, 0) as Decorator;
            _scrollViewer = border?.Child as ScrollViewer;
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

            UpdateLayout();

            if (e?.Action != NotifyCollectionChangedAction.Add)
                return;

            if (e.NewItems.Count != 1)
                return;

            var newItemDataContext = e.NewItems[0];

            if (!GetIsUserMessage(newItemDataContext))
            {
                var indexOfSecondLastItem = Items.Count - 2;
                if (indexOfSecondLastItem <= -1)
                    return;

                if (ItemContainerGenerator.ContainerFromItem(Items[indexOfSecondLastItem]) is FrameworkElement
                    itemContainer)
                {
                    if (!IsItemContainerVisible(itemContainer))
                        return;
                }
            }

            var indexOfLastItem = Items.Count - 1;
            if (indexOfLastItem <= -1)
                return;

            ScrollIntoView(Items[indexOfLastItem]);
        }

        private bool GetIsUserMessage(object dataContext)
        {
            return UserMessageBooleanPath != null &&
                   (bool) dataContext.GetType().GetProperty(UserMessageBooleanPath)?.GetValue(dataContext, null);
        }

        protected bool IsItemContainerVisible(FrameworkElement itemContainer)
        {
            if (_scrollViewer == null)
                return false;

            if (itemContainer == null)
                return false;

            var itemContainerTransform = itemContainer.TransformToAncestor(_scrollViewer);
            var itemContainerRectangle = itemContainerTransform.TransformBounds(new Rect(new Point(0, 0), itemContainer.RenderSize));
            var scrollViewerSize = new Rect(new Point(0, 0), _scrollViewer.RenderSize);
            return scrollViewerSize.IntersectsWith(itemContainerRectangle);
        }
    }
}
