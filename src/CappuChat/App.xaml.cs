using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CappuChat.Configuration;
using Chat.Client;
using Chat.Client.Presenters;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers;
using MahApps.Metro;

namespace CappuChat
{
    public partial class App
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

            ThemeManager.AddAccent("Orgadata", new Uri("pack://application:,,,/CappuChat;component/Styles/OrgadataTheme.xaml"));

            var foundAccent = ThemeManager.Accents.FirstOrDefault(accent =>
                accent.Name.Equals(colorConfiguration.Color, StringComparison.CurrentCultureIgnoreCase));
            var theme = ThemeManager.DetectAppStyle(Current);
            ThemeManager.ChangeAppStyle(Current, foundAccent, theme.Item1);

            Chat.DataAccess.DataAccess.InitializeDatabase();
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
            if (changelog != string.Empty)
                _cappuMainPresenter.ShowChangelog(changelog);
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
                if (fileName.StartsWith(OldPrefix))
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
                    message = message + string.Join(Environment.NewLine, innerException.Message);
                    innerException = innerException.InnerException;
                }
            }
            else
                message = e.Exception.Message;

            if (e.Exception is InvalidOperationException && message.Contains(SignalReconnectingError))
            {
                _viewProvider.ShowMessage(Chat.Texts.Texts.RestartRequired, Chat.Texts.Texts.RestartRequiredServerConnection);
                Process.Start(ResourceAssembly.Location);
                Current.Shutdown();
                Environment.Exit(0);
            }

            _viewProvider.ShowMessage(Chat.Texts.Texts.Error, message);
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
    }
}
