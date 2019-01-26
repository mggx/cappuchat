using Chat.Client.Helper;
using MahApps.Metro;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chat.Client.Windows
{
    /// <summary>
    /// Interaction logic for AccentStyleWindow.xaml
    /// </summary>
    public partial class AccentThemeWindow : MetroWindow
    {
        public static readonly DependencyProperty ColorsProperty
            = DependencyProperty.Register("Colors",
                                          typeof(List<KeyValuePair<string, Color>>),
                                          typeof(AccentThemeWindow),
                                          new PropertyMetadata(default(List<KeyValuePair<string, Color>>)));

        public List<KeyValuePair<string, Color>> Colors
        {
            get { return (List<KeyValuePair<string, Color>>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }

        public AccentThemeWindow()
        {
            InitializeComponent();

            this.DataContext = this;

            this.Colors = typeof(Colors)
                .GetProperties()
                .Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop => new KeyValuePair<String, Color>(prop.Name, (Color)prop.GetValue(null)))
                .ToList();

            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(this, theme.Item2, theme.Item1);
        }

        private void AccentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAccent = AccentSelector.SelectedItem as Accent;
            if (selectedAccent != null)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManager.ChangeAppStyle(Application.Current, selectedAccent, theme.Item1);
                Application.Current.MainWindow.Activate();
            }
        }

        private void ColorsSelectorOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedColor = this.ColorsSelector.SelectedItem as KeyValuePair<string, Color>?;
            if (selectedColor.HasValue)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManagerHelper.CreateAppStyleBy(selectedColor.Value.Value, true);
                Application.Current.MainWindow.Activate();
            }
        }
    }
}