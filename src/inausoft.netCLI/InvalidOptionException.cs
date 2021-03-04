namespace inausoft.netCLI
{
    public class InvalidOptionException : OptionException
    {
        public InvalidOptionException(string commandName, string optionName)
            : base(commandName, optionName, $"Option {optionName} was not defined for command {commandName}.")
        { }
    }
}