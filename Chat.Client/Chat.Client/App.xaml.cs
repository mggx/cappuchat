using Chat.Client.Configuration;
using Chat.Client.Presenters;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers;
using Chat.Models;
using MahApps.Metro;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Chat.Client.Helper;
using System.Windows.Media;

namespace Chat.Client
{
    public partial class App
    {
        private SignalHubConnectionHelper _hubConnectionHelper;
        private ViewProvider _viewProvider;
        private CappuMainPresenter _cappuMainPresenter;

        private void AppOnStartup(object sender, StartupEventArgs e)
        {
            var serverConfigurationController = new ConfigurationController<ServerConfiguration>();
            ServerConfiguration serverConfigurationFile = serverConfigurationController.ReadConfiguration(new ServerConfiguration
            {
                Host = "localhost",
                Port = "1232"
            });

            var colorConfigurationController = new ConfigurationController<ColorConfiguration>();
            var colorConfiguration = colorConfigurationController.ReadConfiguration(new ColorConfiguration
            {
                Color = "Steel"
            });

            ThemeManager.AddAccent("Orgadata", new Uri("pack://application:,,,/Chat.Client;component/Styles/OrgadataTheme.xaml"));

            var foundAccent = ThemeManager.Accents.FirstOrDefault(accent =>
                accent.Name.Equals(colorConfiguration.Color, StringComparison.CurrentCultureIgnoreCase));
            var theme = ThemeManager.DetectAppStyle(Current);
            ThemeManager.ChangeAppStyle(Current, foundAccent, theme.Item1);

            DataAccess.DataAccess.InitializeDatabase();
            _viewProvider = new ViewProvider();

            Current.DispatcherUnhandledException += ApplicationCurrentOnDispatcherUnhandledException;
            Current.Exit += ApplicationCurrentOnExit;

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

            _cappuMainPresenter.Load();
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

            _viewProvider.ShowMessage(Texts.Texts.Error, message);
        }

        private void ApplicationCurrentOnExit(object sender, ExitEventArgs e)
        {
            _hubConnectionHelper.Stop();
            _cappuMainPresenter.CappuLoginPresenter.StartConnection -= LoginPresenterOnStartConnection;
        }

        private Task<bool> LoginPresenterOnStartConnection()
        {
            return _hubConnectionHelper.Start();
        }
    }
}
