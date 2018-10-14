using Chat.Client.SignalHelpers.Contracts.Delegates;
using Chat.Shared.Models;
using System.Threading.Tasks;

namespace Chat.Client.SignalHelpers.Contracts
{
    public interface IVoteSignalHelper
    {
        event VoteCreatedHandler VoteCreated;
        event VoteChangedHandler VoteChanged;

        Task CreateVote(SimpleVote vote);
        Task Vote(bool choice);
    }
}
