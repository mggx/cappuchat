using Chat.Client.ViewModels;
using System.Windows;

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
