using inausoft.netCLI.Deserialization;
using System;
using System.Collections.Generic;

namespace inausoft.netCLI
{
    public class CliConfiguration
    {
        internal readonly Dictionary<Type, Type> _commandMap;

        public IArgumentDeserializer Deserializer { get; private set; }

        public IEnumerable<Type> CommandTypes
        {
            get
            {
                return _commandMap.Keys;
            }
        }

        public CliConfiguration()
        {
            _commandMap = new Dictionary<Type, Type>();
            Deserializer = new RegexArgumentDeserializer();
        }

        public CliConfiguration Map<T1, T2>() where T1 : class where T2 : CommandHandler<T1>
        {
            _commandMap.Add(typeof(T1), typeof(T2));

            return this;
        }

        public CliConfiguration MapDeserialiser(IArgumentDeserializer deserializer)
        {
            Deserializer = deserializer;

            return this;
        }
    }
}
