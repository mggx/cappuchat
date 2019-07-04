using Microsoft.AspNet.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace Chat.Client.SignalHelpers
{
    public sealed class SignalHubConnectionHelper : IDisposable
    {
        private readonly HubConnection _hubConnection;

        public bool Connected { get; private set; }

        public SignalHubConnectionHelper(string connectionString)
        {
            _hubConnection = new HubConnection(connectionString, false);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Catching any and all exception is intended...")]
        public async Task<bool> Start()
        {
            try
            {
                await _hubConnection.Start().ConfigureAwait(false);
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

        #region IDisposable Support
        private bool alreadyDisposed = false; // To detect redundant calls

        public void Dispose(bool disposing)
        {
            if (!alreadyDisposed)
            {
                if (disposing)
                {
                    _hubConnection.Dispose();
                }
                alreadyDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
