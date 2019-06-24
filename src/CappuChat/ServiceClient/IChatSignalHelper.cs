using System.Collections.Generic;
using System.Threading.Tasks;
using CappuChat;
using Chat.Client.SignalHelpers.Contracts.Delegates;

namespace Chat.Client.Signalhelpers.Contracts
{
    public interface IChatSignalHelper
    {
        event MessageReceivedHandler MessageReceivedHandler;
        event MessageReceivedHandler PrivateMessageReceivedHandler;

        Task SendMessage(SimpleMessage message);
        Task SendPrivateMessage(SimpleMessage message);
        Task<IEnumerable<SimpleUser>> GetOnlineUsers();
    }
}
