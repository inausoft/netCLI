using System;

namespace inausoft.netCLI.Deserialization
{
    public interface IOptionsDeserializer
    {
        object Deserialize(Type type, string[] args);

        T Deserialize<T>(string[] args) where T : class;
    }
}
