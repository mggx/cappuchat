namespace Chat.Server.Texts
{
    public static class Texts
    {
        public static string UsernamePasswordCombinationNotFound => "Could not find User with given username / password.";

        public static string CreatingUserFailed(string reason) => $"Could not create User: {reason}";

        public static string CreatingGroupFailed(string reason) => $"Could not create group: {reason}";
        public static string AddingUserToGroupFailed(string reason) => $"Could not add user to group: {reason}";
        public static string DeletingUserFromGroupFailed(string reason) => $"Could not delete user from group: {reason}";

        public static string UserAlreadyExist => "The username you tried to register already exist";
        public static string OtherClientLoggedIn => "Your client was logged out because another client logged in with your username";
        public static string CreatingVoteFailed(string reason) => $"Could not create Vote: {reason}";
        public static string VoteAlreadyCreated => "There is already one active vote.";
        public static string NoActiveVote => "There is no active vote.";
        public static string UserVoteFailed(string reason) => $"Could not vote: {reason}";
        public static string UserAlreadyVoted => "You bastard already voted.";
        public static string InvalidUsername => "The username is invalid.";
        public static string InvalidPassword => "The password is invalid.";
        public static string InvalidGroupName => "The group name is invalid.";
    }
}
