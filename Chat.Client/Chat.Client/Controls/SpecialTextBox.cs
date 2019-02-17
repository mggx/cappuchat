using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Chat.Client.Framework;
using Chat.Client.Styles;
using ChatComponents.Converters;

namespace Chat.Client.Controls
{
    public class SpecialTextBox : TextBox
    {
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.Register(
            "Watermark", typeof(string), typeof(SpecialTextBox), new PropertyMetadata(default(string)));

        public static readonly DependencyProperty FocusedBorderBrushProperty = DependencyProperty.Register(
            "FocusedBorderBrush", typeof(SolidColorBrush), typeof(SpecialTextBox), new PropertyMetadata(default(SolidColorBrush)));

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

        public static readonly DependencyProperty AllowedExtensionsListProperty = DependencyProperty.Register(
            "AllowedExtensionsList", typeof(IList<string>), typeof(SpecialTextBox), new PropertyMetadata(default(IList<string>)));

        public static readonly DependencyProperty DataDroppedCommandProperty = DependencyProperty.Register(
            "DataDroppedCommand", typeof(ICommand), typeof(SpecialTextBox), new PropertyMetadata(default(ICommand)));

        public IList<string> AllowedExtensionsList
        {
            get { return (IList<string>) GetValue(AllowedExtensionsListProperty); }
            set { SetValue(AllowedExtensionsListProperty, value); }
        }

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

        public ICommand DataDroppedCommand
        {
            get { return (ICommand) GetValue(DataDroppedCommandProperty); }
            set { SetValue(DataDroppedCommandProperty, value); }
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

        public SpecialTextBox()
        {
            AllowedExtensionsList = new List<string>();
        }

        protected override void OnPreviewDragOver(DragEventArgs e)
        {
            e.Handled = true;
            if (!(e.Data.GetData(DataFormats.FileDrop, true) is string[] droppedFilePaths)) return;
            var extensions = droppedFilePaths.Select(Path.GetExtension).ToList();
            var isValidData = extensions.All(ext => AllowedExtensionsList.Contains(ext));
            if (isValidData)
            {
                e.Effects = DragDropEffects.Copy;
                return;
            }
            e.Effects = DragDropEffects.None;
        }

        protected override void OnDrop(DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(DataFormats.FileDrop, true)) return;
            if (!(e.Data.GetData(DataFormats.FileDrop, true) is string[] droppedFilePaths)) return;

            foreach (var path in droppedFilePaths)
                DataDroppedCommand?.Execute(path);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            var relayCommand = EnterCommand as RelayCommand<string>;
            relayCommand?.RaiseCanExecuteChanged();
        }
    }
}
