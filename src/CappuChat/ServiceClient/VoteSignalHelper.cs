using CappuChat;
using CappuChat.DTOs;
using Chat.Client.SignalHelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Delegates;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace Chat.Client.SignalHelpers
{
    public class VoteSignalHelper : IVoteSignalHelper
    {
        private readonly IHubProxy _voteHubProxy;

        public event VoteCreatedHandler VoteCreated;
        public event VoteChangedHandler VoteChanged;
        public event Action FinalCappuCalled;

        public VoteSignalHelper(IHubProxy voteHubProxy)
        {
            _voteHubProxy = voteHubProxy ?? throw new ArgumentNullException(nameof(voteHubProxy));

            RegisterHubProxyEvents();
        }

        public void RegisterHubProxyEvents()
        {
            _voteHubProxy.On<SimpleCappuVote>("OnVoteCreated", VoteHubProxyOnVoteCreated);
            _voteHubProxy.On<SimpleCappuVote>("OnVoteChanged", VoteHubProxyOnVoteChanged);
            _voteHubProxy.On("OnFinalCappuCall", VoteHubproxyOnFinalCappuCall);
        }

        private void VoteHubProxyOnVoteCreated(SimpleCappuVote createdVote)
        {
            Application.Current.Dispatcher.Invoke(() => { VoteCreated?.Invoke(createdVote); });
        }

        private void VoteHubProxyOnVoteChanged(SimpleCappuVote changedVote)
        {
            Application.Current.Dispatcher.Invoke(() => { VoteChanged?.Invoke(changedVote); });
        }

        private void VoteHubproxyOnFinalCappuCall()
        {
            Application.Current.Dispatcher.Invoke(() => { FinalCappuCalled?.Invoke(); });
        }

        public async Task CreateVote()
        {
            var serverResponse = await _voteHubProxy.Invoke<SimpleCreateVoteResponse>("CreateCappuVote").ConfigureAwait(false);

            if (!serverResponse.Success)
                throw new CreateVoteFailedException(serverResponse.ErrorMessage);
        }

        public async Task Vote(bool answer)
        {
            var serverResponse = await _voteHubProxy.Invoke<SimpleVoteResponse>("Vote", answer).ConfigureAwait(false);
            if (!serverResponse.Success)
                throw new VoteFailedException(serverResponse.ErrorMessage);
        }

        public async Task<SimpleCappuVote> GetActiveVote()
        {
            var serverResponse = await _voteHubProxy.Invoke<SimpleGetActiveVoteResponse>("GetActiveVote").ConfigureAwait(false);

            if (!serverResponse.Success)
                throw new InvalidOperationException(serverResponse.ErrorMessage);

            return serverResponse.ActiveCappuVote;
        }

        public async Task FinalCappuCall()
        {
            var serverResponse = await _voteHubProxy.Invoke<BaseResponse>("FinalCappuCall").ConfigureAwait(false);

            if (!serverResponse.Success)
                throw new RequestFailedException(serverResponse.ErrorMessage);
        }

        public async Task<IEnumerable<SimpleMessage>> GetVoteScopeMessages()
        {
            var serverResponse = await _voteHubProxy.Invoke<SimpleGetVoteScopeMessagesResponse>("GetVoteScopeMessages").ConfigureAwait(false);
            if (!serverResponse.Success)
                throw new RequestFailedException(serverResponse.ErrorMessage);

            return Application.Current.Dispatcher.Invoke(() => serverResponse.VoteScopeMessages);
        }
    }
}
