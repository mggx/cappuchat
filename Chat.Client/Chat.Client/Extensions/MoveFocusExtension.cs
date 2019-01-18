using System.Windows;
using System.Windows.Input;

namespace Chat.Client.Extensions
{
    public class MoveFocusExtension
    {
        public static readonly DependencyProperty MoveFocusOnEnterProperty = DependencyProperty.RegisterAttached(
            "MoveFocusOnEnter", typeof(bool), typeof(MoveFocusExtension), new PropertyMetadata(PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement frameworkElement)
            {
                frameworkElement.KeyDown += FrameworkElementOnKeydown;
            }
        }

        private static void FrameworkElementOnKeydown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            e.Handled = true;

            var frameworkElement = e.OriginalSource as FrameworkElement;
            if (frameworkElement == null)
                return;

            frameworkElement.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        public static void SetMoveFocusOnEnter(DependencyObject element, bool value)
        {
            element.SetValue(MoveFocusOnEnterProperty, value);
        }

        public static bool GetMoveFocusOnEnter(DependencyObject element)
        {
            return (bool) element.GetValue(MoveFocusOnEnterProperty);
        }
    }
}
