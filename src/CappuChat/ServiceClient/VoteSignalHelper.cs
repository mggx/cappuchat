using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using CappuChat;
using CappuChat.DTOs;
using Chat.Client.SignalHelpers.Contracts;
using Chat.Client.SignalHelpers.Contracts.Delegates;
using Chat.Client.SignalHelpers.Contracts.Exceptions;
using Microsoft.AspNet.SignalR.Client;

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
            if (voteHubProxy == null)
                throw new ArgumentNullException(nameof(voteHubProxy), "Cannot create VoteSignalHelper. Given voteHubProxy is null.");
            _voteHubProxy = voteHubProxy;

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
            var task = _voteHubProxy.Invoke<SimpleCreateVoteResponse>("CreateCappuVote");
            if (task == null)
                throw new NullServerResponseException("Retrieved null task from server.");

            SimpleCreateVoteResponse serverResponse = await task;
            if (!serverResponse.Success)
                throw new CreateVoteFailedException(serverResponse.ErrorMessage);
        }

        public async Task Vote(bool answer)
        {
            var task = _voteHubProxy.Invoke<SimpleVoteResponse>("Vote", answer);
            if (task == null)
                throw new NullServerResponseException("Retrieved null task from server.");

            SimpleVoteResponse serverResponse = await task;
            if (!serverResponse.Success)
                throw new VoteFailedException(serverResponse.ErrorMessage);
        }

        public async Task<SimpleCappuVote> GetActiveVote()
        {
            var task = _voteHubProxy.Invoke<SimpleGetActiveVoteResponse>("GetActiveVote");
            if (task == null)
                throw new NullServerResponseException("Retrieved null task from server.");

            SimpleGetActiveVoteResponse serverResponse = await task;
            if (!serverResponse.Success)
                throw new RequestFailedException(serverResponse.ErrorMessage);

            return serverResponse.ActiveCappuVote;
        }

        public async Task FinalCappuCall()
        {
            var task = _voteHubProxy.Invoke<BaseResponse>("FinalCappuCall");
            if (task == null)
                throw new NullServerResponseException("Retrieved null task from server.");

            BaseResponse serverResponse = await task;
            if (!serverResponse.Success)
                throw new RequestFailedException(serverResponse.ErrorMessage);
        }

        public async Task<IEnumerable<SimpleMessage>> GetVoteScopeMessages()
        {
            var task = _voteHubProxy.Invoke<SimpleGetVoteScopeMessagesResponse>("GetVoteScopeMessages");
            if (task == null)
                throw new NullServerResponseException("Retrieved null task from server.");

            SimpleGetVoteScopeMessagesResponse serverResponse = await task;
            if (!serverResponse.Success)
                throw new RequestFailedException(serverResponse.ErrorMessage);
            return serverResponse.VoteScopeMessages;
        }
    }
}
