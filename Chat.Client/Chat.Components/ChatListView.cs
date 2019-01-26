using System.Collections.Generic;
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
        private bool _loaded;

        public bool LazyScrollToBottom { get; set; }

        public string UserMessageBooleanPath { get; set; }

        public static readonly DependencyProperty UsermessageMenuItemsProperty = DependencyProperty.Register(
            "UsermessageMenuItems", typeof(List<ChatBubbleMenuItem>), typeof(ChatListView), new PropertyMetadata(default(List<ChatBubbleMenuItem>)));

        public static readonly DependencyProperty MessageMenuItemsProperty = DependencyProperty.Register(
            "MessageMenuItems", typeof(List<ChatBubbleMenuItem>), typeof(ChatListView), new PropertyMetadata(default(List<ChatBubbleMenuItem>)));
       
        public static readonly DependencyProperty ItemClickCommandProperty = DependencyProperty.Register(
            "ItemClickCommand", typeof(ICommand), typeof(ChatListView), new PropertyMetadata(default(ICommand)));

        public List<ChatBubbleMenuItem> UsermessageMenuItems
        {
            get { return (List<ChatBubbleMenuItem>) GetValue(UsermessageMenuItemsProperty); }
            set { SetValue(UsermessageMenuItemsProperty, value); }
        }

        public List<ChatBubbleMenuItem> MessageMenuItems
        {
            get { return (List<ChatBubbleMenuItem>) GetValue(MessageMenuItemsProperty); }
            set { SetValue(MessageMenuItemsProperty, value); }
        }

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
            _loaded = true;

            if (VisualChildrenCount > 0)
            {
                Decorator border = VisualTreeHelper.GetChild(this, 0) as Decorator;
                _scrollViewer = border?.Child as ScrollViewer;
            }

            LazyScroll();
        }

        private void LazyScroll()
        {
            if (!LazyScrollToBottom || !_loaded)
                return;
            ScrollToBottom();
            LazyScrollToBottom = false;
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

            LazyScroll();

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
                else
                    return;
            }

            ScrollToBottom();
        }

        public void ScrollToBottom()
        {
            _scrollViewer?.ScrollToBottom();
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

        public IEnumerable<ChatBubbleMenuItem> GetItemContextMenu(object dataContext)
        {
            return GetIsUserMessage(dataContext) ? UsermessageMenuItems : MessageMenuItems;
        }
    }
}
