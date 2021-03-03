using System;

namespace inausoft.netCLI
{
    public class InvalidOptionException : Exception
    {
        public string OptionName { get; }

        public string CommandName { get; }

        public InvalidOptionException(string commandName, string optionName, string message) : base(message)
        {
            OptionName = optionName;
            CommandName = commandName;
        }

        public InvalidOptionException(string commandName, string optionName)
            : this(commandName, optionName, $"Option {optionName} in not defined for command {commandName}") { }
    }
}
