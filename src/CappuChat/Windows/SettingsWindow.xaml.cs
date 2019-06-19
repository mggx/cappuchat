using CappuChat.Configuration;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Chat.Client.Windows
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow
    {
        private readonly IConfigurationController<ServerConfiguration> _serverConfigurationController;
        private readonly IConfigurationController<ColorConfiguration> _colorConfigurationController;

        private readonly Tuple<AppTheme, Accent> _theme;

        private readonly string _actualHost;
        private readonly string _actualPort;

        public static readonly DependencyProperty ColorsProperty = DependencyProperty.Register(
            "Colors", typeof(List<KeyValuePair<string, Color>>),typeof(SettingsWindow), new PropertyMetadata(default(List<KeyValuePair<string, Color>>)));

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "Setter required at runtime")]
        public List<KeyValuePair<string, Color>> Colors
        {
            get { return (List<KeyValuePair<string, Color>>)GetValue(ColorsProperty); }
            set { SetValue(ColorsProperty, value); }
        }

        public SettingsWindow()
        {
            InitializeComponent();

            _serverConfigurationController = new ConfigurationController<ServerConfiguration>();
            _colorConfigurationController = new ConfigurationController<ColorConfiguration>();

            DataContext = this;

            Colors = typeof(Colors)
                .GetProperties()
                .Where(prop => typeof(Color).IsAssignableFrom(prop.PropertyType))
                .Select(prop => new KeyValuePair<String, Color>(prop.Name, (Color)prop.GetValue(null)))
                .ToList();

            _theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(this, _theme.Item2, _theme.Item1);

            AccentSelector.SelectedItem = _theme.Item2;

            ServerConfiguration serverConfigurationFile = _serverConfigurationController.ReadConfiguration();

            TxtBoxHost.Text = serverConfigurationFile.Host;
            TxtBoxPort.Text = serverConfigurationFile.Port;
            FtpUserTextBox.Text = serverConfigurationFile.FtpUser;
            FtpPasswordTextBox.Password = serverConfigurationFile.FtpPassword;

            _actualHost = serverConfigurationFile.Host;
            _actualPort = serverConfigurationFile.Port;
        }

        private void AccentSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!(AccentSelector.SelectedItem is Accent selectedAccent)) return;
            var theme = ThemeManager.DetectAppStyle(Application.Current);
            ThemeManager.ChangeAppStyle(Application.Current, selectedAccent, theme.Item1);
            _colorConfigurationController.WriteConfiguration(new ColorConfiguration { Color = selectedAccent.Name });
        }

        private void CancelSettingsClick(object sender, RoutedEventArgs e)
        {
            ThemeManager.ChangeAppStyle(Application.Current, _theme.Item2, _theme.Item1);
            DialogResult = false;
        }

        private void SaveSettingsClick(object sender, RoutedEventArgs e)
        {
            _serverConfigurationController.WriteConfiguration(new ServerConfiguration
            {
                Host = TxtBoxHost.Text,
                Port = TxtBoxPort.Text,
                FtpUser = FtpUserTextBox.Text,
                FtpPassword = FtpPasswordTextBox.Password
            });

            if (CheckIfConfigHasChanged())
                ShowRestartMessage();
            else
                DialogResult = true;
        }

        private bool CheckIfConfigHasChanged()
        {
            return TxtBoxHost.Text != _actualHost || TxtBoxPort.Text != _actualPort;
        }

        private async void ShowRestartMessage()
        {
            await this.ShowMessageAsync(CappuChat.Properties.Strings.RestartRequired, CappuChat.Properties.Strings.RestartRequiredContent).ConfigureAwait(true);
            DialogResult = true;
            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
