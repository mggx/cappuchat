namespace Chat.Server.Texts
{
    public static class Texts
    {
        public static string UsernamePasswordCombinationNotFound => "Could not find User with given username / password.";
        public static string CreatingUserFailed(string reason) => $"Could not create User: {reason}";
        public static string UserAlreadyExist => "The username you tried to register already exist";
        public static string OtherClientLoggedIn => "Your client was logged out because another client logged in with your username";
    }
}
