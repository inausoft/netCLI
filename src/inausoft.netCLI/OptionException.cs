using System;

namespace inausoft.netCLI
{
    public abstract class OptionException : Exception
    {
        public string OptionName { get; }

        public string CommandName { get; }

        public OptionException(string commandName, string optionName, string message) : base(message)
        {
            OptionName = optionName;
            CommandName = commandName;
        }
    }
}
