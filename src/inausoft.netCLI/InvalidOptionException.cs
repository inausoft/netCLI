using System;

namespace inausoft.netCLI
{
    public class InvalidOptionException : Exception
    {
        public string OptionName { get; }

        public InvalidOptionException(string optionName, string message = null) : base(message)
        {
            OptionName = optionName;
        }
    }
}
