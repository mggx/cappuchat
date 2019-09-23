using System;

namespace CappuChat
{
    [Serializable]
    public class SimpleUser
    {
        public string Username { get; set; }
        public bool IsActive { get; set; }

        public SimpleUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException(CappuChat.Properties.Strings.Error_UserNameCannotBeEmpty, nameof(username));
            Username = username;
        }
    }
}
