using System.Threading.Tasks;
using Chat.Shared.Models;

namespace Chat.Client.Signalhelpers.Contracts
{
    public interface IRegisterSignalHelper
    {
        Task<SimpleUser> Register(string username, string password, byte[] profilePictureData);
    }
}
