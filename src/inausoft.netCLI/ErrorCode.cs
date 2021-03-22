namespace inausoft.netCLI
{
    public enum ErrorCode
    {
        /// <summary>
        /// Unknown error.
        /// </summary>
        Unknown = 1,
        
        /// <summary>
        /// No command name was specified, and no default command was mapped.
        /// </summary>
        UnspecifiedCommand = 10,

        /// <summary>
        /// Supplied command was not mapped.
        /// </summary>
        UnrecognizedCommand = 11,

        UnrecognizedOption = 21,
        InvalidOptionsFormat = 22,

        /// <summary>
        /// At least one of required option was missing.
        /// </summary>
        RequiredOptionMissing = 23,
        OptionValueMissing = 24,
    }
}
