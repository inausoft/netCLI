namespace inausoft.netCLI.Tests.Mocks
{
    [Command("command1")]
    class Command1
    {
        [OptionAttribute("boolOption")]
        public bool BoolOption { get; set; }

        [OptionAttribute("stringOption")]
        public string StringOption { get; set; }

        [OptionAttribute("intOption")]
        public int IntOption { get; set; }

        public string NotOptionProperty { get; set; }
    }
}
