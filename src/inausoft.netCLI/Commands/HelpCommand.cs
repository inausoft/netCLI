namespace inausoft.netCLI.Commands
{
    /// <summary>
    /// Default help command.
    /// </summary>
    [CommandAttribute("help", "Shows details about available commands")]
    public class HelpCommand
    {
        [Option("command", "Command name for which details should be displayed.", IsOptional = true)]
        public string SpecifiedCommandName { get; set; }
    }
}
