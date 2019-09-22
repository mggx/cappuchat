using Chat.Client.ViewModels;
using System.Windows;

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
