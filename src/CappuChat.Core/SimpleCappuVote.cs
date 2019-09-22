using System;
using System.Collections.Generic;

namespace CappuChat
{
    public class SimpleCappuVote
    {
        public Dictionary<string, bool> UserAnswerCache { get; } = new Dictionary<string, bool>();

        public string CreatorName { get; set; }

        public SimpleCappuVote(string creatorName)
        {
            CreatorName = creatorName ?? throw new ArgumentNullException(nameof(creatorName));
        }

        public bool Vote(string username, bool answer)
        {
            if (UserAnswerCache.ContainsKey(username))
                return false;
            UserAnswerCache.Add(username, answer);
            return true;
        }
    }
}
