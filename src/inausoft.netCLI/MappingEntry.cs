using inausoft.netCLI.Deserialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inausoft.netCLI
{
    internal class MappingEntry
    {
        public Type CommandType { get; set; }

        public Type HandlerType { get; set; }

        public object HandlerInstance { get; set; }
    }

    public class Mapping
    {
        internal List<MappingEntry> Entries { get; set; }

        public Mapping()
        {
            Entries = new List<MappingEntry>();
        }

        public Mapping Map<T1, T2>() where T1 : class where T2 : CommandHandler<T1>
        {
            return Map<T1, T2>(null);
        }

        public Mapping Map<T1, T2>(T2 implementation) where T1 : class where T2 : CommandHandler<T1>
        {
            Entries.Add(new MappingEntry()
            {
                CommandType = typeof(T1),
                HandlerType = typeof(T2),
                HandlerInstance = implementation
            });

            return this;
        }

        public IEnumerable<CommandAttribute> CommandInfos
        {
            get
            {
                return Entries.Where(it => Attribute.IsDefined(it.CommandType, typeof(CommandAttribute)))
                               .Select(it => Attribute.GetCustomAttribute(it.CommandType, typeof(CommandAttribute)) as CommandAttribute);
            }
        }
    }
}
