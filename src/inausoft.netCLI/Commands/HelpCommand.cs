namespace inausoft.netCLI.Commands
{
    /// <summary>
    /// Default help command.
    /// </summary>
    [Command("help", "Shows details about available commands.")]
    public class HelpCommand
    {
        /// <summary>
        /// Gets or sets command name for which help should be displayed.
        /// </summary>
        [Option("command", "Command name for which details should be displayed.", IsOptional = true)]
        public string SpecifiedCommandName { get; set; }
    }
}
