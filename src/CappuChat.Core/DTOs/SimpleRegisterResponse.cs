using System;

namespace CappuChat.DTOs
{
    public class SimpleRegisterResponse : BaseResponse
    {
        public SimpleUser User { get; set; }

        public SimpleRegisterResponse()
        {
        }

        public SimpleRegisterResponse(SimpleUser user, bool success, string errorMessage) : base(success, errorMessage)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }
    }
}
