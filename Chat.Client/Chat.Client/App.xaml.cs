﻿using Chat.Client.Configuration;
using Chat.Client.Presenters;
using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers;
using Chat.Client.Windows;
using MahApps.Metro;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace Chat.Client
{
    public partial class App
    {
        private SignalHubConnectionHelper _hubConnectionHelper;
        private ViewProvider _viewProvider;
        private CappuMainPresenter _cappuMainPresenter;
        private IConfigController _configController;
        private ServerConnectionWindow _serverConnectionWindow;

        private void AppOnStartup(object sender, StartupEventArgs e)
        {
            _configController = new ConfigController();
            _configController.DoesConfigFileExists();
            Models.Config config = _configController.ReadConfig();


            DataAccess.DataAccess.InitializeDatabase();
            _viewProvider = new ViewProvider();

            Current.DispatcherUnhandledException += ApplicationCurrentOnDispatcherUnhandledException;
            Current.Exit += ApplicationCurrentOnExit;

            _hubConnectionHelper = new SignalHubConnectionHelper("http://" + config.Host + ":" + config.Port + "/signalr/hubs");

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
