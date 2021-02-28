using System;
using System.Collections.Generic;
using System.Text;

namespace inausoft.netCLI.Deserialization
{
    public interface IArgumentDeserializer
    {
        object Deserialize(Type type, string argumentExpression);
    }
}
