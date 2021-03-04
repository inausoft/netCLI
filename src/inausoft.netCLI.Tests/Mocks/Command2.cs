namespace inausoft.netCLI.Tests.Mocks
{
    [Command("command2")]
    public class Command2
    {
        [Option("stringOption")]
        public string StringOption { get; set; }
    }
}
