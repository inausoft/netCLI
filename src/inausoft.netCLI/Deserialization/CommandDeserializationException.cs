using System;

namespace inausoft.netCLI.Deserialization
{
    public class CommandDeserializationException : Exception
    {
        public ErrorCode ErrorCode { get; }

        public CommandDeserializationException(ErrorCode errorCode, string message = null) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
