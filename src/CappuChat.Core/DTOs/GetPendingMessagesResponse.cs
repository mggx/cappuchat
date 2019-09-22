using System;
using System.Collections.Generic;

namespace CappuChat.DTOs
{
    public class GetPendingMessagesResponse : BaseResponse
    {
        public IEnumerable<SimpleMessage> PendingMessages { get; set; }

        public GetPendingMessagesResponse()
        {
        }

        public GetPendingMessagesResponse(IEnumerable<SimpleMessage> pendingMessages, bool success) : base(success)
        {
            PendingMessages = pendingMessages ?? throw new ArgumentNullException(nameof(pendingMessages));
        }
    }
}
