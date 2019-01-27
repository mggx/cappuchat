using Chat.Client.ViewModels.Events;

namespace Chat.Client.ViewModels.Delegates
{
    public delegate void NewMessagesChangedHandler(object sender, NewMessagesChangedEventArgs e);
}
