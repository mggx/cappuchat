using System;
using System.Windows;
using System.Windows.Controls;

namespace ChatComponents
{
    public class ChatBubble : Control
    {
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text", typeof(string), typeof(ChatBubble), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty TimeProperty = DependencyProperty.Register(
            "Time", typeof(DateTime), typeof(ChatBubble), new PropertyMetadata(default(DateTime)));

        public static readonly DependencyProperty SenderProperty = DependencyProperty.Register(
            "Sender", typeof(string), typeof(ChatBubble), new PropertyMetadata(default(string)));

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

        static ChatBubble()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ChatBubble), new FrameworkPropertyMetadata(typeof(ChatBubble)));
        }
    }
}
