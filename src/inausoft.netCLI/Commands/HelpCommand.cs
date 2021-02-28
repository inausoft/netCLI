namespace inausoft.netCLI.Commands
{
    /// <summary>
    /// Default help command.
    /// </summary>
    [CommandAttribute("help", "Shows details about available commands")]
    public class HelpCommand
    {
        [OptionAttribute("command")]
        public string SpecifiedCommandName { get; set; }
    }
}
