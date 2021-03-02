namespace inausoft.netCLI.Commands
{
    /// <summary>
    /// Default help command.
    /// </summary>
    [Command("help", "Lists available commands and related details.")]
    public class HelpCommand
    {
        [Option("command", "Command name for which details should be displayed.")]
        public string SpecifiedCommandName { get; set; }
    }
}
