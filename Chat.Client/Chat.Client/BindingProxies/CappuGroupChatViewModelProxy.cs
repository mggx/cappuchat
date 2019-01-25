using System.Windows;
using Chat.Client.ViewModels;

namespace Chat.Client.BindingProxies
{
    public class CappuGroupChatViewModelProxy : BindingProxy<CappuGroupChatViewModel>
    {
        protected override Freezable CreateInstanceCore()
        {
            return new CappuGroupChatViewModelProxy();
        }
    }
}
