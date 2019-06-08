using System.Configuration;
using System.Windows;
using System.Windows.Input;

namespace ChatComponents
{
    public class ChatBubbleMenuItem : DependencyObject
    {
        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(string), typeof(ChatBubbleMenuItem), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty DataContextProperty = DependencyProperty.Register(
            "DataContext", typeof(object), typeof(ChatBubbleMenuItem), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(ChatBubbleMenuItem), new PropertyMetadata(default(ICommand)));

        public string Header
        {
            get { return (string) GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public object DataContext
        {
            get { return (object) GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        public ICommand Command
        {
            get { return (ICommand) GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}
