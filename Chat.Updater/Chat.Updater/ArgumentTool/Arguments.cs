using System;
using System.Linq;

namespace Chat.Updater.ArgumentTool
{
    public class Arguments
    {
        private readonly string[] _args;

        public Arguments(string[] args)
        {
            _args = args;
        }

        protected string GetArgument(string argumentName)
        {
            if (!HasArgument(argumentName))
                return string.Empty;

            var argument = _args.FirstOrDefault(arg => arg.Contains(argumentName, StringComparison.CurrentCultureIgnoreCase));
            if (argument == null)
                return string.Empty;

            argument = RemoveChar(argument, '-');
            argument = argument.Remove(argument.IndexOf(argumentName, StringComparison.CurrentCultureIgnoreCase), argumentName.Length);
            return RemoveChar(argument, '=');
        }

        private string RemoveChar(string argument, char charToRemove)
        {
            int index = argument.IndexOf(charToRemove);
            return index < 0 ? argument : argument.Remove(index, 1);
        }

        protected bool HasArgument(string argumentName)
        {
            return _args.Any(arg => arg.Contains(argumentName, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
