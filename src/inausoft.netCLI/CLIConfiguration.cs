using inausoft.netCLI.Deserialization;
using System;
using System.Collections.Generic;

namespace inausoft.netCLI
{
    public class CLIConfiguration
    {
        internal readonly Dictionary<Type, Type> _commandMap;

        public IOptionsDeserializer Deserializer { get; private set; }

        public IEnumerable<Type> CommandTypes
        {
            get
            {
                return _commandMap.Keys;
            }
        }

        public CLIConfiguration()
        {
            _commandMap = new Dictionary<Type, Type>();
            Deserializer = new RegexOptionsDeserializer();
        }

        public CLIConfiguration Map<T1, T2>() where T1 : class where T2 : CommandHandler<T1>
        {
            _commandMap.Add(typeof(T1), typeof(T2));

            return this;
        }

        public CLIConfiguration MapDeserializer(IOptionsDeserializer deserializer)
        {
            Deserializer = deserializer;

            return this;
        }
    }
}