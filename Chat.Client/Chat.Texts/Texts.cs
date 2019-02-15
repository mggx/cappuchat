namespace Chat.Texts
{
    public class Texts
    {
        public static string Ok => "Ok";
        public static string Cancel => "Cancel";

        public static string TypeMessage => "Type a message...";
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

        public static string CappuCallOverview => "Cappu Vote - Overview";
        public static string CappuCalled => "Cappu Call! ANSWER NOW!!";
        public static string GoCall => "Go";
        public static string GoGoCall => "GOGOOGOGOGOOGOGOGOGOIOGOGOGO";
        public static string Chats => "Chats";
        public static string NoChatSelected => "No chat selected";

        public static string QuestionWatermark => "Whats the question?";
        public static string AnswerWatermark => "Whats the answer?";
        public static string Answers => "Answers";
        public static string GoQuestion => "Go?";
        public static string UsersWhoAnswered => "Users which answered:";
        public static string OnlineUsers => "Online Users";

        public static string CreateVote => "Cappu Call";

        public static string TitleVoteAlreadyCreated => "Already...";
        public static string VoteAlreadyCreated => "Too late.. vote already created by someone else...";

        public static string NotImplementedYet => "Not implemented yet.";

        public static string ServerSettings => "Server settings";
        public static string ServerUrlWatermark => "Server Url...";
        public static string ServerPortWatermark => "Server Port...";
        public static string Save => "Save";

        public static string Voted => "Voted";
        public static string NotVoted => "Not voted yet";
        public static string SomethingWentWrong => "Something went wrong...";

        public static string ServerConnectionSetting => "Serverconnection & FTP Settings";
        public static string ColorSettings => "Colorsettings";
        public static string TestConnection => "Test connection";
        public static string RestartRequired => "Restart is required";
        public static string RestartRequiredConent => "Due to your changes you need to restart the client!";
        public static string AppSettingsWindowTitle => "Settings";
        public static string ApplicationSettings => "Applicationsettings";

        public static string AutoCheckForUpdates => "Check for Updates on startup";

        public static string PushNotificationsOn => "Push notifications on";
        public static string PushNotificationsOff => "Push notifications off";
    }
}