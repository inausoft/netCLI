using inausoft.netCLI;
using System;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Represents sets of mappings between commands and related command handlers.
/// </summary>
public class CommandMapping
{
    private List<MappingEntry> _entries { get; set; }

    internal MappingEntry DefaultEntry { get; set; }

    internal IEnumerable<MappingEntry> Entries
    {
        get
        {
            var entries = new List<MappingEntry>(_entries);
            if (DefaultEntry != null)
            {
                entries.Add(DefaultEntry);
            }
            return entries;
        }
    }

    /// <summary>
    /// Initializes a new instance of <see cref="CommandMapping"/>.
    /// </summary>
    public CommandMapping()
    {
        _entries = new List<MappingEntry>();
    }

    /// <summary>
    /// Maps specified command with supplied <see cref="ICommandHandler"/>.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="THandler"></typeparam>
    /// <returns></returns>
    public CommandMapping Map<TCommand, THandler>() where TCommand : class where THandler : CommandHandler<TCommand>
    {
        if (Entries.Any(it => it.CommandType == typeof(TCommand)))
        {
            throw new InvalidOperationException($"'{typeof(TCommand)}' was already mapped.");
        }

        _entries.Add(new MappingEntry()
        {
            CommandType = typeof(TCommand),
            HandlerType = typeof(THandler),
        });

        return this;
    }

    /// <summary>
    /// Maps specified command with supplied <see cref="ICommandHandler"/>.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="THandler"></typeparam>
    /// <returns></returns>
    public CommandMapping MapDefault<TCommand, THandler>() where TCommand : class where THandler : CommandHandler<TCommand>
    {
        if (DefaultEntry != null)
        {
            throw new InvalidOperationException("Default command was already mapped.");
        }

        if (Entries.Any(it => it.CommandType == typeof(TCommand)))
        {
            throw new InvalidOperationException($"'{typeof(TCommand)}' was already mapped.");
        }

        DefaultEntry = new MappingEntry()
        {
            CommandType = typeof(TCommand),
            HandlerType = typeof(THandler),
        };

        return this;
    }

    /// <summary>
    /// Maps specified command with supplied <see cref="ICommandHandler"/> instance.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="THandler"></typeparam>
    /// <returns></returns>
    public CommandMapping Map<TCommand>(CommandHandler<TCommand> implementation) where TCommand : class
    {
        if (Entries.Any(it => it.CommandType == typeof(TCommand)))
        {
            throw new InvalidOperationException($"'{typeof(TCommand)}' was already mapped.");
        }

        _entries.Add(new MappingEntry()
        {
            CommandType = typeof(TCommand),
            HandlerType = implementation.GetType(),
            HandlerInstance = implementation
        });

        return this;
    }

    /// <summary>
    /// Maps specified command with supplied <see cref="ICommandHandler"/> instance.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <typeparam name="THandler"></typeparam>
    /// <returns></returns>
    public CommandMapping MapDefault<TCommand>(CommandHandler<TCommand> implementation) where TCommand : class
    {
        if (DefaultEntry != null)
        {
            throw new InvalidOperationException("Default command was already mapped.");
        }

        if (Entries.Any(it => it.CommandType == typeof(TCommand)))
        {
            throw new InvalidOperationException($"'{typeof(TCommand)}' was already mapped.");
        }

        DefaultEntry = new MappingEntry()
        {
            CommandType = typeof(TCommand),
            HandlerType = implementation.GetType(),
            HandlerInstance = implementation
        };

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