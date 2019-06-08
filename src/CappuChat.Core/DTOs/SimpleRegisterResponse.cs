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
            if (user == null)
                throw new ArgumentNullException(nameof(user), "Cannot create SimpleRegisterResponse. Given user is null.");
            User = user;
        }
    }
}
