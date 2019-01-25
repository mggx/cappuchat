using Chat.Client.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Chat.Client.Windows
{
    /// <summary>
    /// Interaction logic for PrivateMessageWindow.xaml
    /// </summary>
    public partial class PrivateMessageWindow
    {
        public PrivateMessageWindow()
        {
            InitializeComponent();
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var privateMessageViewModel = DataContext as CappuChatViewModel;
            privateMessageViewModel?.SendMessageCommand?.Execute(MessageInput.Text);
            MessageInput.Clear();
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            var privateMessageViewModel = DataContext as CappuChatViewModel;
            privateMessageViewModel?.SendMessageCommand?.Execute(MessageInput.Text);
            MessageInput.Clear();
        }
    }
}
