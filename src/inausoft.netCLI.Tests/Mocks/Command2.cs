namespace inausoft.netCLI.Tests.Mocks
{
    [Command("command2")]
    public class Command2
    {
        public const string SomeDefaultValue = "SomeDefaultValue";

        [Option("stringOption", IsOptional = true)]
        public string StringOption { get; set; }

        public Command2()
        {
            StringOption = SomeDefaultValue;
        }
    }
}
