using System;

namespace inausoft.netCLI.Deserialization
{
    public class DeserializationException : Exception
    {
        public ErrorCode ErrorCode { get; }

        public DeserializationException(ErrorCode errorCode, string message) : base(message)
        {
            ErrorCode = errorCode;
        }
    }
}
