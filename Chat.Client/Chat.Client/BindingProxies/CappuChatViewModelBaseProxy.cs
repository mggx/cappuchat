using System.Windows;
using Chat.Client.ViewModels;

namespace Chat.Client.BindingProxies
{
    public class CappuChatViewModelBaseProxy : BindingProxy<CappuChatViewModelBase>
    {
        protected override Freezable CreateInstanceCore()
        {
            return new CappuChatViewModelBaseProxy();
        }
    }
}
