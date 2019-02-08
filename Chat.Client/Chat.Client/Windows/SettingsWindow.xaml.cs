using Chat.Client.Configuration;
using Chat.Client.Helper;
using MahApps.Metro;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Diagnostics;
using ConfigurationFile = Chat.Models.Configuration;

namespace Chat.Client.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        private Tuple<AppTheme, Accent> _theme;

        private IConfigurationController _configurationController;

        private string _actualHost;
        private string _actualPort;

        public static readonly DependencyProperty ColorsProperty = DependencyProperty.Register(
            "Colors", typeof(List<KeyValuePair<string, Color>>),typeof(SettingsWindow), new PropertyMetadata(default(List<KeyValuePair<string, Color>>)));

        public List<KeyValuePair<string, Color>> Colors
        {
            get { return (List<KeyValuePair<string, Color>>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }

        public SettingsWindow()
        {
            InitializeComponent();
            _configurationController = new ConfigurationController();

            this.DataContext = this;

            this.Colors = typeof(Colors)
                .GetProperties()
                .Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop => new KeyValuePair<String, Color>(prop.Name, (Color)prop.GetValue(null)))
                .ToList();

            _theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(this, _theme.Item2, _theme.Item1);

            AccentSelector.SelectedItem = _theme.Item2;
            ColorsSelector.SelectedItem = _theme.Item1;

            ConfigurationFile configurationFile = _configurationController.ReadConfiguration();

            TxtBoxHost.Text = configurationFile.Host;
            TxtBoxPort.Text = configurationFile.Port;

            _actualHost = configurationFile.Host;
            _actualPort = configurationFile.Port;
        }

        

        private void AccentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAccent = AccentSelector.SelectedItem as Accent;
            if (selectedAccent != null)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManager.ChangeAppStyle(Application.Current, selectedAccent, theme.Item1);
            }
        }

        private void ColorsSelectorOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedColor = this.ColorsSelector.SelectedItem as KeyValuePair<string, Color>?;
            if (selectedColor.HasValue)
            {
                var theme = ThemeManager.DetectAppStyle(Application.Current);
                ThemeManagerHelper.CreateAppStyleBy(selectedColor.Value.Value, true);
            }
        }

        private void CancelSettingsClick(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, _theme.Item2, _theme.Item1);
            DialogResult = false;
        }

        private void SaveSettingsClick(object sender, RoutedEventArgs e)
        {
            _configurationController.WriteConfiguration(new ConfigurationFile() { Host = TxtBoxHost.Text, Port = TxtBoxPort.Text });
            if (CheckIfConfigHasChanged())
                ShowRestartMessage();
            else
                DialogResult = true;
        }

        private bool CheckIfConfigHasChanged()
        {
            if (TxtBoxHost.Text != _actualHost || TxtBoxPort.Text != _actualPort)
                return true;
            else
                return false;
        }

        private async void ShowRestartMessage()
        {
            await this.ShowMessageAsync(Texts.Texts.RestartRequired, Texts.Texts.RestartRequiredConent);
            DialogResult = true;
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
