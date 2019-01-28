using Chat.Client.Framework;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;

namespace ChatComponents
{
    public class ChatBubble : Control
    {
        private ChatListView _chatListView;
        private DropDownButton _dropDownButton;
        private Button _reactionButton;
        private TextBlock _senderTextBlock;
        private Badged _reactionBadged;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(ChatBubble), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(DateTime), typeof(ChatBubble), new PropertyMetadata(default(DateTime)));

        public static readonly DependencyProperty SenderProperty = DependencyProperty.Register(
            "Sender", typeof(string), typeof(ChatBubble), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ReactionsProperty = DependencyProperty.Register(
            "Reactions", typeof(int), typeof(ChatBubble), new PropertyMetadata(default(int)));

        public DateTime Text
        {
            get { return (DateTime) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        
        public DateTime Time
        {
            get { return (DateTime) GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        public string Sender
        {
            get { return (string) GetValue(SenderProperty); }
            set { SetValue(SenderProperty, value); }
        }

        public int Reactions
        {
            get { return (int)GetValue(ReactionsProperty); }
            set { SetValue(ReactionsProperty, value); }
        }

        static ChatBubble()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChatBubble), new FrameworkPropertyMetadata(typeof(ChatBubble)));
        }

        public ChatBubble()
        {
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _chatListView = FindAncestor<ChatListView>(this);
            _dropDownButton = Template.FindName("PART_DropDownButton", this) as DropDownButton;
            _reactionButton = Template.FindName("PART_ReactionButton", this) as Button;
            _senderTextBlock = Template.FindName("PART_SenderTextBlock", this) as TextBlock;
            _reactionBadged = Template.FindName("PART_ReactionBadged", this) as Badged;
            if (_dropDownButton == null) return;
            _dropDownButton.Click += DropDownButtonOnClick;
            _reactionButton.Click += ReactionButtonClick;
            _dropDownButton.ItemsSource = _chatListView.GetItemContextMenu(DataContext);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _dropDownButton.Click -= DropDownButtonOnClick;
            _reactionButton.Click -= ReactionButtonClick;
        }

        private void DropDownButtonOnClick(object sender, RoutedEventArgs e)
        {
            _chatListView.SelectedItem = DataContext;
        }

        private void ReactionButtonClick(object sender, RoutedEventArgs e)
        {
            if (_reactionBadged.Badge == null)
                _reactionBadged.Badge = new Badged();

            _reactionBadged.Badge = 1;
        }

        public static T FindAncestor<T>(DependencyObject child)
            where T : DependencyObject
        {
            while (child != null)
            {
                T objTest = child as T;
                if (objTest != null)
                    return objTest;
                child = VisualTreeHelper.GetParent(child);
            }
            return null;
        }
    }
}
