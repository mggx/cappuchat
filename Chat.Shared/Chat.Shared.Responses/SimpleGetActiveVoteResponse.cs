using System;
using Chat.Shared.Models;

namespace Chat.Responses
{
    public class SimpleGetActiveVoteResponse : BaseResponse
    {
        public SimpleVote ActiveVote { get; set; }

        public SimpleGetActiveVoteResponse()
        {
        }

        public SimpleGetActiveVoteResponse(SimpleVote activeVote, bool success) : base(success)
        {
            ActiveVote = activeVote;
        }

        public SimpleGetActiveVoteResponse(SimpleVote activeVote, bool success, string errorMessage) : base(success, errorMessage)
        {
            ActiveVote = activeVote;
        }
    }
}
