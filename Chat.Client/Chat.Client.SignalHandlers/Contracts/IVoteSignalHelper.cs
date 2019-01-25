using Chat.Client.SignalHelpers.Contracts.Delegates;
using Chat.Shared.Models;
using System;
using System.Threading.Tasks;

namespace Chat.Client.SignalHelpers.Contracts
{
    public interface IVoteSignalHelper
    {
        event VoteCreatedHandler VoteCreated;
        event VoteChangedHandler VoteChanged;
        event Action FinalCappuCalled;

        Task CreateVote();
        Task Vote(bool answer);
        Task<SimpleCappuVote> GetActiveVote();
        Task FinalCappuCall();
    }
}
