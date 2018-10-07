using System.Windows;
using System.Windows.Controls;

namespace Chat.Client.Extensions
{
    public class TextBoxWatermarkExtension
    {
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached(
            "Watermark", typeof(string), typeof(TextBoxWatermarkExtension), new PropertyMetadata(default(string), PropertyChangedCallback));

        private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox callerTextBox = (TextBox) d;
            callerTextBox.Loaded += CallerTextBoxOnLoaded;
        }

        private static void CallerTextBoxOnLoaded(object sender, RoutedEventArgs e)
        {
            TextBox callerTextBox = (TextBox) sender;
            TextBlock waterMarkTextBlock = callerTextBox.Template?.FindName("PART_Watermark", callerTextBox) as TextBlock;
            waterMarkTextBlock.Text = GetWatermark(callerTextBox);
        }

        public static void SetWatermark(DependencyObject element, string value)
        {
            element.SetValue(WatermarkProperty, value);
        }

        public static string GetWatermark(DependencyObject element)
        {
            return (string) element.GetValue(WatermarkProperty);
        }
    }
}
