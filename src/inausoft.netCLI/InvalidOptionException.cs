namespace inausoft.netCLI
{
    public class InvalidOptionException : OptionException
    {
        public string OptionName { get; }

        public InvalidOptionException(string optionName, string message = null) : base(message)
        {
            OptionName = optionName;
        }
    }
}