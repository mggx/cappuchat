using Chat.Shared.Models;

namespace Chat.Responses
{
    public class SimpleCreateVoteResponse : BaseResponse
    {
        public SimpleCreateVoteResponse()
        {
        }

        public SimpleCreateVoteResponse(bool success) : base(success)
        {
        }

        public SimpleCreateVoteResponse(bool success, string errorMessage) : base(success, errorMessage)
        {
        }
    }
}
