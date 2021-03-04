namespace inausoft.netCLI
{
    public class MissingOptionException : OptionException
    {
        public MissingOptionException(string commandName, string optionName)
            : base(commandName, optionName, $"Required option {optionName} was not provided for command {commandName}.")
        { }
    }
}
