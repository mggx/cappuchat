using Chat.Client.ViewModels;
using System.Windows;

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
