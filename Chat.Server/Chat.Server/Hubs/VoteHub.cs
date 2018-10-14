using System.Collections.Generic;
using Chat.Responses;
using Chat.Shared.Models;

namespace Chat.Server.Hubs
{
    public class VoteHub : BaseHub
    {
        public static SimpleVote ActiveVote;

        public SimpleCreateVoteResponse CreateVote(SimpleVote createdVote)
        {
            SimpleCreateVoteResponse response = new SimpleCreateVoteResponse();

            if (ActiveVote == null)
            {
                string username = GetUsernameByConnectionId(Context.ConnectionId);
                createdVote.CreatorName = username;
                ActiveVote = createdVote;
                response.Success = true;
                Clients.All.OnVoteCreated(ActiveVote);
            }
            else
            {
                response.Success = false;
                response.ErrorMessage = Texts.Texts.CreatingVoteFailed(Texts.Texts.VoteAlreadyCreated);
            }

            return response;
        }

        public SimpleVoteResponse Vote(int answerId)
        {
            SimpleVoteResponse response = new SimpleVoteResponse { Success = true };

            string username = GetUsernameByConnectionId(Context.ConnectionId);
            if (ActiveVote.Vote(username, answerId))
                Clients.All.OnVoteAnswersChanged(ActiveVote);
            else
            {
                response.Success = false;
                response.ErrorMessage = Texts.Texts.UserVoteFailed(Texts.Texts.UserAlreadyVoted);
            }

            return response;
        }
    }
}
