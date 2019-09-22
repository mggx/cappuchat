using CappuChat;
using CappuChat.DTOs;

namespace Chat.Server.Hubs
{
    public class VoteHub : BaseHub
    {
        public static SimpleCappuVote ActiveCappuVote { get; private set; }

        public SimpleCreateVoteResponse CreateCappuVote()
        {
            SimpleCreateVoteResponse response = new SimpleCreateVoteResponse();

            if (ActiveCappuVote == null)
            {
                string username = GetUsernameByConnectionId(Context.ConnectionId);
                SimpleCappuVote cappuVote = new SimpleCappuVote(username);
                ActiveCappuVote = cappuVote;
                Clients.All.OnVoteCreated(cappuVote);
            }
            else
            {
                response.Success = false;
                response.ErrorMessage = "The requested vote was already created.";
            }

            return response;
        }

        public SimpleVoteResponse Vote(bool answer)
        {
            SimpleVoteResponse response = new SimpleVoteResponse();

            if (ActiveCappuVote == null)
            {
                response.Success = false;
                response.ErrorMessage = "No active vote was found";
                return response;
            }

            string username = GetUsernameByConnectionId(Context.ConnectionId);
            if (ActiveCappuVote.Vote(username, answer))
            {
                InvokeOnVoteChanged();
            }
            else
            {
                response.Success = false;
                response.ErrorMessage = "Vote failed because the user already voted.";
            }

            return response;
        }

        public SimpleGetActiveVoteResponse GetActiveVote()
        {
            return new SimpleGetActiveVoteResponse(ActiveCappuVote, true);
        }

        public BaseResponse FinalCappuCall()
        {
            BaseResponse response = new BaseResponse(true);
            Clients.All.OnFinalCappuCall();
            ActiveCappuVote = null;
            InvokeOnVoteChanged();
            return response;
        }

        public void InvokeOnVoteChanged()
        {
            Clients.All.OnVoteChanged(ActiveCappuVote);
        }

        //public SimpleGetVoteScopeMessagesResponse GetVoteScopeMessages()
        //{
        //    SimpleGetVoteScopeMessagesResponse response = new SimpleGetVoteScopeMessagesResponse();
        //    return response;
        //}
    }
}
