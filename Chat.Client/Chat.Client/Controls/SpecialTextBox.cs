using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Chat.Client.Styles;

namespace Chat.Client.Controls
{
    public class SpecialTextBox : TextBox
    {
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark", typeof(string), typeof(SpecialTextBox), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.Register(
            "FocusedBorderBrush", typeof(SolidColorBrush), typeof(SpecialTextBox), new PropertyMetadata(ProgramColors.AccentDarkColor1));

        public string Watermark
        {
            get { return (string) GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public SolidColorBrush FocusedBorderBrush
        {
            get { return (SolidColorBrush) GetValue(FocusedBorderBrushProperty); }
            set { SetValue(FocusedBorderBrushProperty, value); }
        }

        static SpecialTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SpecialTextBox), new FrameworkPropertyMetadata(typeof(SpecialTextBox)));
        }
    }
}
