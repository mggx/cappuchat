using System.Collections.Generic;
using System.Windows;
using Chat.Client.Helper;
using Chat.Client.ViewModels;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using ChatComponents;

namespace Chat.Client.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl
    {
        private WindowKeyDownHelper _windowKeyDownHelper;

        public static readonly DependencyProperty UsermessageMenuItemsProperty = DependencyProperty.Register(
            "UsermessageMenuItems", typeof(List<ChatBubbleMenuItem>), typeof(ChatView), new PropertyMetadata(default(List<ChatBubbleMenuItem>)));

        public static readonly DependencyProperty MessageMenuItemsProperty = DependencyProperty.Register(
            "MessageMenuItems", typeof(List<ChatBubbleMenuItem>), typeof(ChatView), new PropertyMetadata(default(List<ChatBubbleMenuItem>)));

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

        public ChatView()
        {
            UsermessageMenuItems = new List<ChatBubbleMenuItem>();
            MessageMenuItems = new List<ChatBubbleMenuItem>();

            InitializeComponent();
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _windowKeyDownHelper = new WindowKeyDownHelper(this);
            _windowKeyDownHelper.Register();
            _windowKeyDownHelper.KeyDown += WindowOnKeyDown;

            InputTextBox.Focus();
        }

        private void WindowOnKeyDown(object sender, KeyEventArgs e)
        {
            if (!Keyboard.IsKeyDown(Key.LeftAlt) || !Keyboard.IsKeyDown(Key.C))
                return;

            var viewModel = DataContext as CappuChatViewModelBase;
            viewModel?.ClearMessagesCommand?.Execute(null);
        }

        private void OnUnloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (_windowKeyDownHelper == null)
                return;

            _windowKeyDownHelper.Unregister();
            _windowKeyDownHelper.KeyDown -= WindowOnKeyDown;
            _windowKeyDownHelper = null;
        }

        private void SendMessageButtonOnClick(object sender, RoutedEventArgs e)
        {
            var chatViewModel = DataContext as CappuChatViewModelBase;

            chatViewModel?.SendMessageCommand?.Execute(InputTextBox.Text);

            InputTextBox.Clear();
            InputTextBox.Focus();

            chatViewModel?.SendMessageCommand?.RaiseCanExecuteChanged();
        }

        private void SendSpongeMessageButtonOnClick(object sender, RoutedEventArgs e)
        {
            var chatViewModel = DataContext as CappuChatViewModelBase;

            chatViewModel?.SendSpongeMessageCommand?.Execute(InputTextBox.Text);

            InputTextBox.Clear();
            InputTextBox.Focus();

            chatViewModel?.SendSpongeMessageCommand?.RaiseCanExecuteChanged();
        }
    }
}
