using System.Windows;
using Chat.Client.Presenters;

namespace Chat.Client.BindingProxies
{
    public class CappuMainPresenterProxy : BindingProxy<CappuMainPresenter>
    {
        protected override Freezable CreateInstanceCore()
        {
            return new CappuMainPresenterProxy();
        }
    }
}
