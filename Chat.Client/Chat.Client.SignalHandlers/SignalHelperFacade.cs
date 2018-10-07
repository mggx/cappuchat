using Chat.Client.Signalhelpers.Contracts;

namespace Chat.Client.SignalHelpers
{
    public class SignalHelperFacade : ISignalHelperFacade
    {
        public IRegisterSignalHelper RegisterSignalHelper { get; set; }
        public ILoginSignalHelper LoginSignalHelper { get; set; }
        public IChatSignalHelper ChatSignalHelper { get; set; }
    }
}
