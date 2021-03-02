using System;

namespace inausoft.netCLI.Deserialization
{
    public interface IOptionsDeserializer
    {
        object Deserialize(Type type, string[] args);
    }
}
