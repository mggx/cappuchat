using System.Collections.Generic;

namespace CappuChat.DTOs
{
    public class SimpleGetVoteScopeMessagesResponse : BaseResponse
    {
        public IList<SimpleMessage> VoteScopeMessages { get; set; } 
    }
}
