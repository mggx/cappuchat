using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chat.Client.Extensions
{
    public class PasswordBoxFocusedBehaviorExtension
    {
        public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.RegisterAttached(
            "FocusedBorderBrush", typeof(SolidColorBrush), typeof(PasswordBoxFocusedBehaviorExtension), new PropertyMetadata(default(SolidColorBrush), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox callerPasswordBox = (PasswordBox) d;
            callerPasswordBox.IsKeyboardFocusWithinChanged += CallerPasswordBoxIsKeyboardFocusWithinChanged;
            callerPasswordBox.GotKeyboardFocus += CallerPasswordBox_GotKeyboardFocus;
        }

        private static void CallerPasswordBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            PasswordBox callerPasswordBox = (PasswordBox)sender;
            callerPasswordBox.BorderBrush = GetFocusedBorderBrush(callerPasswordBox);
        }

        private static void CallerPasswordBoxIsKeyboardFocusWithinChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox callerPasswordBox = (PasswordBox) sender;
            var boolean = (bool) e.NewValue;
            if (boolean)
                callerPasswordBox.BorderBrush = GetFocusedBorderBrush(callerPasswordBox);
        }

        public static void SetFocusedBorderBrush(DependencyObject element, SolidColorBrush value)
        {
            element.SetValue(FocusedBorderBrushProperty, value);
        }

        public static SolidColorBrush GetFocusedBorderBrush(DependencyObject element)
        {
            return (SolidColorBrush) element.GetValue(FocusedBorderBrushProperty);
        }
    }
}
