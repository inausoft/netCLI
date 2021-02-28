using System;

namespace inausoft.netCLI
{
    public class InvalidCommandException : Exception
    {
        public string CommandName { get; }

        public InvalidCommandException(string commandName, string message = null) : base(message)
        {
            CommandName = commandName;
        }
    }
}
