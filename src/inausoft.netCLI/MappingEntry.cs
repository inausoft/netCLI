using inausoft.netCLI.Deserialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inausoft.netCLI
{
    public class CommandInfo
    {
        public CommandAttribute Command { get; internal set; }

        public IEnumerable<OptionAttribute> Options { get; internal set; }
    }

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
            Entries.Add(new MappingEntry()
            {
                CommandType = typeof(T1),
                HandlerType = typeof(T2),
            });

            return this;
        }

        public Mapping Map<T1>(CommandHandler<T1> implementation) where T1 : class
        {
            Entries.Add(new MappingEntry()
            {
                CommandType = typeof(T1),
                HandlerType = implementation.GetType(),
                HandlerInstance = implementation
            });

            return this;
        }

        public IEnumerable<CommandInfo> CommandInfos
        {
            get
            {
                return Entries.Where(it => Attribute.IsDefined(it.CommandType, typeof(CommandAttribute)))
                               .Select(it =>
                               {
                                   return new CommandInfo()
                                   {
                                       Command = Attribute.GetCustomAttribute(it.CommandType, typeof(CommandAttribute)) as CommandAttribute,
                                       Options = it.CommandType.GetProperties().Where(property => Attribute.IsDefined(property, typeof(OptionAttribute)))
                                                                                .Select(property => Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute)
                                   };
                               });
            }
        }
    }
}
