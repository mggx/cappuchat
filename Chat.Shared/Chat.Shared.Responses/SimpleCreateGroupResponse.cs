using Chat.Shared.Models;

namespace Chat.Responses
{
    public class SimpleCreateGroupResponse : BaseResponse
    {
        public SimpleGroup Group { get; set; }

        public SimpleCreateGroupResponse()
        {
        }

        public SimpleCreateGroupResponse(bool success) : base(success)
        {
        }

        public SimpleCreateGroupResponse(bool success, string errorMessage) : base(success, errorMessage)
        {
        }
    }
}
