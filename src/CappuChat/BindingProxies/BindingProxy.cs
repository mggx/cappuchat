using System.Windows;

namespace Chat.Client.BindingProxies
{
    public class BindingProxy<T> : Freezable
    {
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register(
            "Data", typeof(T), typeof(BindingProxy<T>), new PropertyMetadata(default(T)));

        public T Data {
            get { return (T)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy<T>();
        }
    }
}
