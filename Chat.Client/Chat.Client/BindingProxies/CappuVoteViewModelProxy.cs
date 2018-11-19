using System.Windows;
using Chat.Client.ViewModels;

namespace Chat.Client.BindingProxies
{
    public class CappuVoteViewModelProxy : BindingProxy<CappuVoteViewModel>
    {
        protected override Freezable CreateInstanceCore()
        {
            return new CappuVoteViewModelProxy();
        }
    }
}
