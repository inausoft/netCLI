using System;

namespace inausoft.netCLI
{
    public class InvalidOptionException : Exception
    {
        public string OptionName { get; }

        public string CommandName { get; }

        public InvalidOptionException(string commandName, string optionName, string message = null) : base(message)
        {
            OptionName = optionName;
            CommandName = commandName;
        }
    }
}
