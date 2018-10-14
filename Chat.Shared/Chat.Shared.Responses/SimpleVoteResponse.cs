namespace Chat.Responses
{
    public class SimpleVoteResponse : BaseResponse
    {
        public SimpleVoteResponse()
        {
        }

        public SimpleVoteResponse(bool success) : base(success)
        {
        }

        public SimpleVoteResponse(bool success, string errorMessage) : base(success, errorMessage)
        {
        }
    }
}
