namespace inausoft.netCLI.Tests.Mocks
{
    [Command("command-sample")]
    public class Command3
    {
        public const string SomeDefaultValue = "SomeDefaultValue";

        [Option("stringOption", IsOptional = true)]
        public string StringOption { get; set; }

        public Command3()
        {
            StringOption = SomeDefaultValue;
        }
    }
}
