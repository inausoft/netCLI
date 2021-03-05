namespace inausoft.netCLI
{
    public enum ErrorCode
    {
        None = 0,
        Unknown = 1,
        
        UnspecifiedCommand = 10,
        UnrecognizedCommand = 11,

        UnrecognizedOption = 21,
        InvalidOptionsFormat = 22,
        RequiredOptionMissing = 23,
    }
}
