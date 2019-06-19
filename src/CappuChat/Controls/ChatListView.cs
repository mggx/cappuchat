using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ChatComponents
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Dependency property setters are required at runtime... i think //TODO")]
    public class ChatListView : ListView
    {
        private ScrollViewer _scrollViewer;

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
            if (VisualChildrenCount > 0)
            {
                Decorator border = VisualTreeHelper.GetChild(this, 0) as Decorator;
                _scrollViewer = border?.Child as ScrollViewer;
            }

            ScrollToBottom();
        }

        private void LazyScroll()
        {
            if (!LazyScrollToBottom)
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
            var type = dataContext.GetType();
            var property = type.GetProperty(UserMessageBooleanPath);
            var value = property?.GetValue(dataContext);
            return value != null && (bool) value;
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
