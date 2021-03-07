using System;
using System.Collections.Generic;
using System.Linq;

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

        public Mapping Map<TCommand, THandler>() where TCommand : class where THandler : CommandHandler<TCommand>
        {
            Entries.Add(new MappingEntry()
            {
                CommandType = typeof(TCommand),
                HandlerType = typeof(THandler),
            });

            return this;
        }

        public Mapping Map<TCommand>(CommandHandler<TCommand> implementation) where TCommand : class
        {
            Entries.Add(new MappingEntry()
            {
                CommandType = typeof(TCommand),
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
