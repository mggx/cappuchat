using Chat.Client.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;

namespace ChatComponents
{
    public class ChatBubble : Control
    {
        private ChatListView _chatListView;
        private DropDownButton _dropDownButton;

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(ChatBubble), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(DateTime), typeof(ChatBubble), new PropertyMetadata(default(DateTime)));

        public static readonly DependencyProperty SenderProperty = DependencyProperty.Register(
            "Sender", typeof(string), typeof(ChatBubble), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty ImageSourceProperty = DependencyProperty.Register(
            "ImageSource", typeof(MemoryStream), typeof(ChatBubble), new PropertyMetadata(default(MemoryStream)));

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

        public MemoryStream ImageSource
        {
            get { return (MemoryStream) GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
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

            var bubbleImage = Template.FindName("BubbleImage", this) as Image;
            if (bubbleImage != null && ImageSource != null)
            {
                MaxWidth = bubbleImage.ActualWidth;
            }

            if (_dropDownButton == null) return;
            _dropDownButton.Click += DropDownButtonOnClick;
            _dropDownButton.ItemsSource = _chatListView.GetItemContextMenu(DataContext);
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _dropDownButton.Click -= DropDownButtonOnClick;
        }

        private void DropDownButtonOnClick(object sender, RoutedEventArgs e)
        {
            _chatListView.SelectedItem = DataContext;
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
