using System.Threading.Tasks;
using CappuChat;

namespace Chat.Client.Signalhelpers.Contracts
{
    public interface IRegisterSignalHelper
    {
        Task<SimpleUser> Register(string username, string password);
    }
}
