using System;
using System.Windows;
using System.Windows.Input;

namespace Chat.Client.Helper
{
    public class WindowKeyDownHelper
    {
        private readonly FrameworkElement _frameworkElement;
        private readonly Window _window;

        public event EventHandler<KeyEventArgs> KeyDown;

        public WindowKeyDownHelper(FrameworkElement frameworkElement)
        {
            _frameworkElement = frameworkElement ?? throw new ArgumentNullException(nameof(frameworkElement));
            _window = GetWindow();
        }

        private Window GetWindow()
        {
            var window = Window.GetWindow(_frameworkElement);
            if (window == null)
                throw new InvalidOperationException(
                    $"Could not register WindowKeyDownHelper. Given frameworkElement {_frameworkElement} has no parent window.");
            return window;
        }

        public void Register()
        {
            _window.PreviewKeyDown += WindowOnKeyDown;
        }

        public void Unregister()
        {
            _window.PreviewKeyDown -= WindowOnKeyDown;
        }

        private void WindowOnKeyDown(object sender, KeyEventArgs e)
        {
            KeyDown?.Invoke(sender, e);
        }
    }
}
