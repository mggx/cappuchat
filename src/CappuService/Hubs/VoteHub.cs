using CappuChat;
using CappuChat.DTOs;

namespace Chat.Server.Hubs
{
    public class VoteHub : BaseHub
    {
        public static SimpleCappuVote ActiveCappuVote;

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
                response.ErrorMessage = Texts.Texts.CreatingVoteFailed(Texts.Texts.VoteAlreadyCreated);
            }

            return response;
        }

        public SimpleVoteResponse Vote(bool answer)
        {
            SimpleVoteResponse response = new SimpleVoteResponse();

            if (ActiveCappuVote == null)
            {
                response.Success = false;
                response.ErrorMessage = Texts.Texts.NoActiveVote;
                return response;
            }

            string username = GetUsernameByConnectionId(Context.ConnectionId);
            if (ActiveCappuVote.Vote(username, answer))
                InvokeOnVoteChanged();
            else
            {
                response.Success = false;
                response.ErrorMessage = Texts.Texts.UserVoteFailed(Texts.Texts.UserAlreadyVoted);
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
