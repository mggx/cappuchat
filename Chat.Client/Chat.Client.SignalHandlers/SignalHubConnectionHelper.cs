using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace Chat.Client.SignalHelpers
{
    public class SignalHubConnectionHelper
    {
        private readonly HubConnection _hubConnection;

        public bool Connected { get; private set; }

        public SignalHubConnectionHelper(string connectionString)
        {
            _hubConnection = new HubConnection(connectionString, false);
        }

        public async Task<bool> Start()
        {
            try
            {
                await _hubConnection.Start();
            }
            catch (Exception)
            {
                Connected = false;
                return Connected;
            }

            Connected = true;
            return Connected;
        }

        public IHubProxy CreateHubProxy(string hubName)
        {
            return _hubConnection.CreateHubProxy(hubName);
        }

        public void Stop()
        {
            _hubConnection.Stop();
            Connected = false;
        }
    }
}
