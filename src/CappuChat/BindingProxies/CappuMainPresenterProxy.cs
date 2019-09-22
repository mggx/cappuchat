using Chat.Client.Presenters;
using System.Windows;

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
