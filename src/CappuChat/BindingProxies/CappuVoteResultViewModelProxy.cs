using Chat.Client.ViewModels;
using System.Windows;

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
