using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

        public static readonly DependencyProperty IsAdditionalButtonVisibleProperty = DependencyProperty.Register(
            "IsAdditionalButtonVisible", typeof(bool), typeof(SpecialTextBox), new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty AdditionalButtonContentProperty = DependencyProperty.Register(
            "AdditionalButtonContent", typeof(object), typeof(SpecialTextBox), new PropertyMetadata(default(object)));

        public static readonly DependencyProperty AdditionalButtonCommandProperty = DependencyProperty.Register(
            "AdditionalButtonCommand", typeof(ICommand), typeof(SpecialTextBox), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty EnterCommandProperty = DependencyProperty.Register(
            "EnterCommand", typeof(ICommand), typeof(SpecialTextBox), new PropertyMetadata(default(ICommand)));

        public static readonly DependencyProperty WatermarkForegroundProperty = DependencyProperty.Register(
            "WatermarkForeground", typeof(SolidColorBrush), typeof(SpecialTextBox), new PropertyMetadata(default(SolidColorBrush)));

        public SolidColorBrush WatermarkForeground
        {
            get { return (SolidColorBrush) GetValue(WatermarkForegroundProperty); }
            set { SetValue(WatermarkForegroundProperty, value); }
        }

        public ICommand EnterCommand
        {
            get { return (ICommand) GetValue(EnterCommandProperty); }
            set { SetValue(EnterCommandProperty, value); }
        }

        public ICommand AdditionalButtonCommand
        {
            get { return (ICommand) GetValue(AdditionalButtonCommandProperty); }
            set { SetValue(AdditionalButtonCommandProperty, value); }
        }

        public object AdditionalButtonContent
        {
            get { return (object) GetValue(AdditionalButtonContentProperty); }
            set { SetValue(AdditionalButtonContentProperty, value); }
        }

        public bool IsAdditionalButtonVisible
        {
            get { return (bool) GetValue(IsAdditionalButtonVisibleProperty); }
            set { SetValue(IsAdditionalButtonVisibleProperty, value); }
        }

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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key != Key.Enter)
                return;

            if (EnterCommand == null)
                return;

            if (!EnterCommand.CanExecute(Text))
                return;

            EnterCommand.Execute(Text);
            Text = string.Empty;
        }
    }
}
