using CappuChat;
using System.Threading.Tasks;

namespace Chat.Client.Signalhelpers.Contracts
{
    public interface IRegisterSignalHelper
    {
        Task<SimpleUser> Register(string username, string password);
    }
}
