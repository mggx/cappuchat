using CappuChat.ServiceClient;
using Chat.Client.SignalHelpers.Contracts;

namespace Chat.Client.Signalhelpers.Contracts
{
    public interface ISignalHelperFacade
    {
        IRegisterSignalHelper RegisterSignalHelper { get; set; }
        ILoginSignalHelper LoginSignalHelper { get; set; }
        IChatSignalHelper ChatSignalHelper { get; set; }
        IVoteSignalHelper VoteSignalHelper { get; set; }
        IUserStatusSignalHelper UserStatusSignalHelper { get; set; }
    }
}
