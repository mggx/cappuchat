using System;

namespace Chat.Shared.Models
{
    [Serializable]
    public class SimpleUser
    {
        public string Username { get; set; }

        public SimpleUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username), "Cannot create SimpleUser. Given username is invalid.");
            Username = username;
        }
    }
}
