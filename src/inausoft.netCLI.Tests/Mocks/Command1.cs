namespace inausoft.netCLI.Tests.Mocks
{
    [Command("command1")]
    class Command1
    {
        [Option("boolOption")]
        public bool BoolOption { get; set; }

        [Option("stringOption")]
        public string StringOption { get; set; }

        [Option("intOption")]
        public int IntOption { get; set; }

        public string NotOptionProperty { get; set; }
    }
}
