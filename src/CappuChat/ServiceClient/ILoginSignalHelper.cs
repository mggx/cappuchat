using System;
using System.Threading.Tasks;
using Chat.Client.SignalHelpers.Contracts.Delegates;
using CappuChat;

namespace Chat.Client.Signalhelpers.Contracts
{
    public interface ILoginSignalHelper
    {
        SimpleUser User { get; }
        event EventHandler<string> ConnectionIdChanged;
        event EventHandler<string> LoggedOutByServer;
        event OnlineUsersChangedHandler OnlineUsersChanged;

        Task<SimpleUser> Login(string username, string password);
        Task Logout();
    }
}
