using inausoft.netCLI.Deserialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace inausoft.netCLI
{
    public class CLIFlow
    {
        protected internal IServiceProvider ServiceProvider { get; set; }
        protected internal CommandMapping Mapping { get; set; }
        protected internal ICommandDeserializer Deserializer { get; set; }
        protected internal Func<ErrorCode, int> FallbackFunc { get; set; }

        protected internal CLIFlow() { }

        /// <summary>
        /// Deserializes supplied arguments into command and runs mapped <see cref="ICommandHandler"/>.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public int Run(string[] args)
        {
            var mapping = Mapping ?? ServiceProvider.GetService<CommandMapping>() ?? throw new InvalidOperationException();

            MappingEntry mappingEntry;

            if (args.Any() && !args[0].StartsWith("-"))
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
                    return FallbackFunc(ErrorCode.UnspecifiedCommand);
                }

                mappingEntry = mapping.DefaultEntry;
            }

            if (mappingEntry == null)
            {
                return FallbackFunc(ErrorCode.UnrecognizedCommand);
            }

            object command;

            try
            {
                command = Deserializer.Deserialize(mappingEntry.CommandType, args.ToArray());
            }
            catch (CommandDeserializationException ex)
            {
                return FallbackFunc(ex.ErrorCode);
            }
            catch (Exception)
            {
                return FallbackFunc(ErrorCode.Unknown);
            }

            var handler = (mappingEntry.HandlerInstance ?? ServiceProvider.GetRequiredService(mappingEntry.HandlerType)) as ICommandHandler;

            return handler.Run(command);
        }
    }

    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection ConfigureCLIFlow(this IServiceCollection services, Action<CommandMapping> setup)
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
