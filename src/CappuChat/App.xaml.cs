using CappuChat.Configuration;
using Chat.Client;
using Chat.Client.Presenters;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers;
using MahApps.Metro;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CappuChat
{
    public partial class App : Application, IDisposable
    {
        private const string SignalReconnectingError = "Data cannot be sent because the WebSocket connection is reconnecting.";
        private const string OldPrefix = "old_";

        private SignalHubConnectionHelper _hubConnectionHelper;
        private ViewProvider _viewProvider;
        private CappuMainPresenter _cappuMainPresenter;

        private void AppOnStartup(object sender, StartupEventArgs e)
        {
            Current.DispatcherUnhandledException += ApplicationCurrentOnDispatcherUnhandledException;
            Current.Exit += ApplicationCurrentOnExit;

            RemoveOldFiles();

            var serverConfigurationController = new ConfigurationController<ServerConfiguration>();
            ServerConfiguration serverConfigurationFile = serverConfigurationController.ReadConfiguration(new ServerConfiguration
            {
                Host = "localhost",
                Port = "1232",
                FtpUser = "cappuftp",
                FtpPassword = "cappuftp1234"
            });

            var colorConfigurationController = new ConfigurationController<ColorConfiguration>();
            var colorConfiguration = colorConfigurationController.ReadConfiguration(new ColorConfiguration
            {
                Color = "Steel"
            });

            ThemeManager.AddAccent("Orgadata", new Uri("Styles/OrgadataTheme.xaml", UriKind.Relative));

            var foundAccent = ThemeManager.Accents.FirstOrDefault(accent =>
                accent.Name.Equals(colorConfiguration.Color, StringComparison.CurrentCultureIgnoreCase));
            var theme = ThemeManager.DetectAppStyle(Current);
            ThemeManager.ChangeAppStyle(Current, foundAccent, theme.Item1);

            Chat.DataAccess.DatabaseClient.InitializeDatabase();
            _viewProvider = new ViewProvider();

            _hubConnectionHelper = new SignalHubConnectionHelper("http://" + serverConfigurationFile.Host + ":" + serverConfigurationFile.Port + "/signalr/hubs");

            ISignalHelperFacade signalHelperFacade = new SignalHelperFacade
            {
                ChatSignalHelper = new ChatSignalHelper(_hubConnectionHelper.CreateHubProxy("ChatHub")),
                LoginSignalHelper = new LoginSignalHelper(_hubConnectionHelper.CreateHubProxy("LoginHub")),
                RegisterSignalHelper = new RegisterSignalHelper(_hubConnectionHelper.CreateHubProxy("RegisterHub")),
                VoteSignalHelper = new VoteSignalHelper(_hubConnectionHelper.CreateHubProxy("VoteHub"))
            };

            _cappuMainPresenter = new CappuMainPresenter(signalHelperFacade, _viewProvider)
            {
                CappuLoginPresenter = { ConnectedToServer = _hubConnectionHelper.Connected }
            };

            _cappuMainPresenter.CappuLoginPresenter.StartConnection += LoginPresenterOnStartConnection;

            _viewProvider.Show(_cappuMainPresenter);

            var changelog = GetChangelog();
            if (!string.IsNullOrEmpty(changelog))
                _cappuMainPresenter.ShowChangelog(changelog);
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("de-DE");
            _cappuMainPresenter.Load();
        }

        private string GetChangelog()
        {
            var changelogFilePath = $@"{Environment.CurrentDirectory}\changelog.txt";
            if (!File.Exists(changelogFilePath))
                return string.Empty;

            var changelog = File.ReadAllText(changelogFilePath, Encoding.Default);
            File.Delete(changelogFilePath);
            return changelog;
        }

        private void RemoveOldFiles()
        {
            foreach (var process in Process.GetProcessesByName("Chat.Updater"))
            {
                process.Kill();
                process.WaitForExit(1000);
            }

            foreach (var file in Directory.GetFiles(Environment.CurrentDirectory))
            {
                var fileName = Path.GetFileName(file);
                if (fileName.StartsWith(OldPrefix, StringComparison.OrdinalIgnoreCase))
                    File.Delete(fileName);
            }
        }

        private void ApplicationCurrentOnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            string message = string.Empty;

            if (string.IsNullOrWhiteSpace(e.Exception.Message))
            {
                Exception innerException = e.Exception.InnerException;

                while (innerException != null)
                {
                    message += string.Join(Environment.NewLine, innerException.Message);
                    innerException = innerException.InnerException;
                }
            }
            else
            {
                message = e.Exception.Message;
            }

            if (e.Exception is InvalidOperationException && message.Contains(SignalReconnectingError))
            {
                _viewProvider.ShowMessage(CappuChat.Properties.Strings.RestartRequired, CappuChat.Properties.Strings.RestartRequiredServerConnection);
                Process.Start(ResourceAssembly.Location);
                Current.Shutdown();
                Environment.Exit(0);
            }

            _viewProvider.ShowMessage(CappuChat.Properties.Strings.Error, message);
        }

        private void ApplicationCurrentOnExit(object sender, ExitEventArgs e)
        {
            _hubConnectionHelper.Stop();
            _cappuMainPresenter.CappuLoginPresenter.StartConnection -= LoginPresenterOnStartConnection;
            _cappuMainPresenter.Dispose();
        }

        private Task<bool> LoginPresenterOnStartConnection()
        {
            return _hubConnectionHelper.Start();
        }

        #region IDisposable Support
        private bool alreadyDisposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!alreadyDisposed)
            {
                if (disposing)
                {
                    _cappuMainPresenter.Dispose();
                    _viewProvider.Dispose();
                    _hubConnectionHelper.Dispose();
                }
                alreadyDisposed = true;
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "No one's gonna derive from App, mate...")]
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
