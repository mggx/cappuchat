using Chat.Client.Signalhelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts;

namespace Chat.Client.SignalHelpers
{
    public class SignalHelperFacade : ISignalHelperFacade
    {
        public IRegisterSignalHelper RegisterSignalHelper { get; set; }
        public ILoginSignalHelper LoginSignalHelper { get; set; }
        public IChatSignalHelper ChatSignalHelper { get; set; }
        public IVoteSignalHelper VoteSignalHelper { get; set; }
    }
}
