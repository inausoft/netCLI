using inausoft.netCLI.Deserialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace inausoft.netCLI
{
    public class CLIFlow
    {
        private IServiceProvider _serviceProvider;

        private CommandMapping _mapping;

        private ICommandDeserializer _deserializer;

        private Action<ErrorCode> _fallbackFunc;

        internal CLIFlow()
        {
            _deserializer = new LogicalCommandDeserializer();
            _fallbackFunc = (error) => { };
        }

        public CLIFlow UseFallback(Action<ErrorCode> fallback)
        {
            _fallbackFunc = fallback ?? throw new ArgumentNullException(nameof(fallback));
            return this;
        }

        /// <summary>
        /// Instructs to explicitly use specified <see cref="CommandMapping"/>.
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public CLIFlow UseMapping(CommandMapping mapping)
        {
            _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
            return this;
        }

        /// <summary>
        /// Instructs to use <see cref="IServiceProvider"/> to create instances of mapped <see cref="ICommandHandler"/>.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public CLIFlow UseServiceProvider(IServiceProvider provider)
        {
            _serviceProvider = provider ?? throw new ArgumentOutOfRangeException(nameof(provider));
            return this;
        }

        /// <summary>
        /// Instructs to use supplied <see cref="ICommandDeserializer"/> instead of default implementation for the purpose of deserializing command arguments.
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        public CLIFlow UseDeserializer(ICommandDeserializer deserializer)
        {
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            return this;
        }

        /// <summary>
        /// Deserializes supplied arguments into command and runs mapped <see cref="ICommandHandler"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public int Run(string[] args)
        {
            if (_mapping == null && _serviceProvider == null)
            {
                throw new InvalidOperationException($"{nameof(CommandMapping)} need to be provided either with {nameof(UseMapping)} method or via {nameof(IServiceProvider)}.");
            }

            var mapping = _mapping ?? _serviceProvider.GetService<CommandMapping>() ?? throw new InvalidOperationException();

            MappingEntry mappingEntry;

            if (args.Any() && !args[0].Contains("-"))
            {
                mappingEntry = mapping.Entries.FirstOrDefault(it =>
                 Attribute.IsDefined(it.CommandType, typeof(CommandAttribute)) &&
                 (Attribute.GetCustomAttribute(it.CommandType, typeof(CommandAttribute)) as CommandAttribute).Name == args[0]);

                args = args.Skip(1).ToArray();
            }
            else
            {
                if (mapping.DefaultEntry == null)
                {
                    return Fallback(ErrorCode.UnspecifiedCommand);
                }

                mappingEntry = mapping.DefaultEntry;
            }

            if (mappingEntry == null)
            {
                return Fallback(ErrorCode.UnrecognizedCommand);
            }

            object command;

            try
            {
                command = _deserializer.Deserialize(mappingEntry.CommandType, args.ToArray());
            }
            catch (CommandDeserializationException ex)
            {
                return Fallback(ex.ErrorCode);
            }
            catch (Exception)
            {
                return Fallback(ErrorCode.Unknown);
            }

            var handler = (mappingEntry.HandlerInstance ?? _serviceProvider.GetRequiredService(mappingEntry.HandlerType)) as ICommandHandler;

            return handler.Run(command);
        }

        private int Fallback(ErrorCode errorCode)
        {
            _fallbackFunc(errorCode);

            return (int)errorCode;
        }

        public static CLIFlow Create()
        {
            return new CLIFlow();
        }
    }

    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection ConfigureCLFlow(this IServiceCollection services, Action<CommandMapping> setup)
        {
            if (services == null)
            {
                throw new ArgumentNullException($"{nameof(services)} cannot be null.");
            }

            if (setup == null)
            {
                throw new ArgumentNullException($"{nameof(setup)} cannot be null.");
            }

            CommandMapping mapping = new CommandMapping();
            setup(mapping);
            services.AddSingleton(mapping);

            mapping.Entries.ToList().ForEach(it =>
            {
                if (it.HandlerInstance != null)
                {
                    services.AddSingleton(it.HandlerInstance);
                }
                else
                {
                    services.AddSingleton(it.HandlerType);
                }
            });
            
            return services;
        }
    }
}
