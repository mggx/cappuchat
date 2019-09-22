using System;

namespace CappuUpdater.ArgumentTool
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "//TODO: WTF does this even mean...? Where does the assemblyPath come from?")]
        private bool GetArgument(string argumentName, out string argument)
        {
            var retrievedArgument = GetArgument(argumentName);
            if (string.IsNullOrWhiteSpace(retrievedArgument))
                throw new ArgumentException("Could not resolve assemblyPath from arguments.");
            retrievedArgument = retrievedArgument.Replace("\"", string.Empty);
            argument = retrievedArgument.TrimEnd();
            return true;
        }

        public UpdaterArguments(string[] args) : base(args)
        {
        }
    }
}
