using System.Security.Policy;

namespace Chat.Texts
{
    public class Texts
    {
        public static string Ok => "Ok";
        public static string Cancel => "Cancel";

        public static string SendMessage => "Send message";
        public static string Friends => "Friends";
        public static string UsernameWatermark => "Whats your username?...";
        public static string PasswordWatermark => "Whats your password?...";

        public static string Login => "Login";
        public static string LoggingIn => "Logging in...";
        public static string LoginSuccessful => "Login successful";
        public static string LoginFailed => "Login failed";
        public static string LoginFailedReason(string reason) => $"Login failed. Reason: {reason}";


        public static string Logout => "Logout";
        public static string LoggedOut => "Logged out";
        public static string LoggedOutReason(string reason) => $"You were logged out. Reason: {reason}";

        public static string Register => "Register";
        public static string RegisterSuccessful => "Registered successfully";
        public static string RegisterFailed => "Register failed";
        public static string RegisterFailedReason(string reason) => $"Register failed. Reason: {reason}";

        public static string ConnectedToServer => "Connected to server";
        public static string NotConnectedToServer => "Not connected to server";
        public static string ClickToReconnect => "Click to try reconnecting";

        public static string Error => "Error";

        public static string CappuCallOverview => "Cappu Call Overview";
        public static string CappuCalled => "Cappu Call! ANSWER NOW!!";
        public static string GoCall => "Go";
        public static string GoGoCall => "GOGOOGOGOGOOGOGOGOGOIOGOGOGO";
        public static string Chats => "Chats";

        public static string QuestionWatermark => "Whats the question?";
        public static string AnswerWatermark => "Whats the answer?";
        public static string Answers => "Answers";
        public static string GoQuestion => "Go?";
        public static string UsersWhoAnswered => "Users which answered:";
        public static string OnlineUsers => "Online Users";

        public static string CreateVote => "Cappu Call";
    }
}
