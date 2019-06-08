namespace CappuChat.DTOs
{
    public class SimpleGetActiveVoteResponse : BaseResponse
    {
        public SimpleCappuVote ActiveCappuVote { get; set; }

        public SimpleGetActiveVoteResponse()
        {
        }

        public SimpleGetActiveVoteResponse(SimpleCappuVote activeCappuVote, bool success) : base(success)
        {
            ActiveCappuVote = activeCappuVote;
        }

        public SimpleGetActiveVoteResponse(SimpleCappuVote activeCappuVote, bool success, string errorMessage) : base(success, errorMessage)
        {
            ActiveCappuVote = activeCappuVote;
        }
    }
}
