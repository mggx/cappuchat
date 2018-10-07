using System;

namespace Chat.Shared.Models
{
    public class SimpleUser
    {
        public string Username { get; set; }

        public SimpleUser()
        {
        }

        public SimpleUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username), "Cannot create SimpleUser. Given username is invalid.");
            Username = username;
        }
    }
}
