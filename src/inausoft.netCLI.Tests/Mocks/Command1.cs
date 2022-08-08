namespace inausoft.netCLI.Tests.Mocks
{
    [Command("command1")]
    class Command1
    {
        [Option("boolOption|b")]
        public bool BoolOption { get; set; }

        [Option("stringOption|s")]
        public string StringOption { get; set; }

        [Option("intOption|i")]
        public int IntOption { get; set; }

        public string NotOptionProperty { get; set; }
    }
}
