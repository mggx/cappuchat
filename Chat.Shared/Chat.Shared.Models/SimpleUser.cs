using System;

namespace Chat.Shared.Models
{
    [Serializable]
    public class SimpleUser
    {
        public string Username { get; set; }

        public byte[] ProfilePictureData { get; set; }

        public SimpleUser(string username, byte[] profilePictureData)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentNullException(nameof(username), "Cannot create SimpleUser. Given username is invalid.");

            if (profilePictureData == null)
                throw new ArgumentNullException(nameof(profilePictureData), "Cannot create SimpleUser. Given profilpicturedata is invalid.");
            Username = username;
            ProfilePictureData = profilePictureData;
        }
    }
}
