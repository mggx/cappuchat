using System.Windows;
using System.Windows.Controls;

namespace Chat.Client.UserControls
{
    /// <summary>
    /// Interaction logic for UserContainer.xaml
    /// </summary>
    public partial class UserContainer : UserControl
    {
        public static readonly DependencyProperty NewMessagesProperty = DependencyProperty.Register(
            "NewMessages", typeof(int), typeof(UserContainer), new PropertyMetadata(default(int)));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register(
            "Header", typeof(string), typeof(UserContainer), new PropertyMetadata(default(string)));

        public new static readonly DependencyProperty ContentProperty = DependencyProperty.Register(
            "Content", typeof(object), typeof(UserContainer), new PropertyMetadata(default(object)));

        public new static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
            "IsActive", typeof(bool), typeof(UserContainer), new PropertyMetadata(default(bool)));

        public int NewMessages {
            get { return (int)GetValue(NewMessagesProperty); }
            set { SetValue(NewMessagesProperty, value); }
        }

        public string Header {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public new object Content {
            get { return GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        public bool IsActive
        {
            get { return (bool)GetValue(IsActiveProperty); }
            set { SetValue(IsActiveProperty, value); }
        }

        public UserContainer()
        {
            InitializeComponent();
        }
    }
}
