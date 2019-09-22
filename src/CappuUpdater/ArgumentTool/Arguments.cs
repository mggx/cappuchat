using System;
using System.Linq;

namespace CappuUpdater.ArgumentTool
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
            if (string.IsNullOrEmpty(argumentName))
                throw new ArgumentNullException(nameof(argumentName));

            if (HasArgument(argumentName))
            {
                var argument = _args.FirstOrDefault(arg => arg.Contains(argumentName, StringComparison.CurrentCultureIgnoreCase));
                if (!string.IsNullOrEmpty(argument))
                {
                    argument = RemoveChar(argument, '-');
                    if (!string.IsNullOrEmpty(argument))
                    {
                        var pos = argument.IndexOf(argumentName, StringComparison.CurrentCultureIgnoreCase);
#pragma warning disable CA1062 // Validate arguments of public methods
                        argument = argument.Remove(pos, argumentName.Length); //For some fucking reason FxCop won't shut up about this one...
#pragma warning restore CA1062 // Validate arguments of public methods
                        return RemoveChar(argument, '=');
                    }
                }
            }
            return string.Empty;
        }

        private static string RemoveChar(string argument, char charToRemove)
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
