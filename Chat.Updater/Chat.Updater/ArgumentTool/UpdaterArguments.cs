using System;

namespace Chat.Updater.ArgumentTool
{
    public class UpdaterArguments : Arguments
    {
        private const string AssemblyPath = "AssemblyPath";
        private const string Host = "Host";
        private const string FtpUser = "FtpUser";
        private const string FtpPassword = "FtpPassword";

        public string GetAssemblyPath => GetArgument(AssemblyPath, out string retrievedArgument) ? retrievedArgument : string.Empty;
        public string GetHost => GetArgument(Host, out string retrievedArgument) ? retrievedArgument : string.Empty;
        public string GetFtpUser => GetArgument(FtpUser, out string retrievedArgument) ? retrievedArgument : string.Empty;
        public string GetFtpPassword => GetArgument(FtpPassword, out string retrievedArgument) ? retrievedArgument : string.Empty;

        private bool GetArgument(string argumentName, out string argument)
        {
            var retrievedArgument = GetArgument(argumentName);
            if (string.IsNullOrWhiteSpace(retrievedArgument))
                throw new ArgumentException("Could not resolve assemblyPath from arguments.");
            argument = retrievedArgument;
            return true;
        }

        public UpdaterArguments(string[] args) : base(args)
        {
        }
    }
}
