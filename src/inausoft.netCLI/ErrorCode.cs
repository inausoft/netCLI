using System;
using System.Collections.Generic;
using System.Text;

namespace inausoft.netCLI
{
    public enum ErrorCode
    {
        None = 0,
        Unknown = 1,
        UnspecifiedCommand = 10,
        UnrecognizedCommand = 11,
        UnrecognizedOption = 12,
        UnrecognizedPattern = 13,
    }
}
