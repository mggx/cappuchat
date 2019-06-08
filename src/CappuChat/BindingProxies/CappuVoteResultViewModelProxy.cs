using System.Windows;
using Chat.Client.ViewModels;

namespace Chat.Client.BindingProxies
{
    public class CappuVoteResultViewModelProxy : BindingProxy<CappuVoteResultViewModel>
    {
        protected override Freezable CreateInstanceCore()
        {
            return new CappuVoteResultViewModelProxy();
        }
    }
}
