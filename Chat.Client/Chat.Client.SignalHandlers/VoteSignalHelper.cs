using System;
using System.Threading.Tasks;
using System.Windows;
using Chat.Client.SignalHelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Delegates;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Chat.Responses;
using Chat.Shared.Models;
using Microsoft.AspNet.SignalR.Client;

namespace Chat.Client.SignalHelpers
{
    public class VoteSignalHelper : IVoteSignalHelper
    {
        private readonly IHubProxy _voteHubProxy;

        public event VoteCreatedHandler VoteCreated;
        public event VoteChangedHandler VoteChanged;

        public VoteSignalHelper(IHubProxy voteHubProxy)
        {
            if (voteHubProxy == null)
                throw new ArgumentNullException(nameof(voteHubProxy), "Cannot create VoteSignalHelper. Given voteHubProxy is null.");
            _voteHubProxy = voteHubProxy;

            RegisterHubProxyEvents();
        }

        public void RegisterHubProxyEvents()
        {
            _voteHubProxy.On<SimpleVote>("OnVoteCreated", VoteHubProxyOnVoteCreated);
            _voteHubProxy.On<SimpleVote>("OnVoteChanged", VoteHubProxyOnVoteChanged);
        }
        
        private void VoteHubProxyOnVoteCreated(SimpleVote createdVote)
        {
            Application.Current.Dispatcher.Invoke(() => { VoteCreated?.Invoke(createdVote); });
        }

        private void VoteHubProxyOnVoteChanged(SimpleVote changedVote)
        {
            Application.Current.Dispatcher.Invoke(() => { VoteChanged?.Invoke(changedVote); });
        }

        public async Task CreateVote(SimpleVote vote)
        {
            var task = _voteHubProxy.Invoke<SimpleCreateVoteResponse>("CreateVote", vote);
            if (task == null)
                throw new NullServerResponseException("Retrieved null task from server.");

            SimpleCreateVoteResponse serverResponse = await task;
            if (!serverResponse.Success)
                throw new CreateVoteFailedException(serverResponse.ErrorMessage);
        }

        public async Task Vote(bool choice)
        {
            var task = _voteHubProxy.Invoke<SimpleVoteResponse>("Vote", choice);
            if (task == null)
                throw new NullServerResponseException("Retrieved null task from server.");

            SimpleVoteResponse serverResponse = await task;
            if (!serverResponse.Success)
                throw new CreateVoteFailedException(serverResponse.ErrorMessage);
        }
    }
}
