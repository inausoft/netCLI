using inausoft.netCLI.Deserialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace inausoft.netCLI
{
    public class CLFlow
    {
        private IServiceProvider _serviceProvider;

        private Mapping _config;

        private IOptionsDeserializer _deserializer;

        private Action<ErrorCode> _fallbackFunc;

        internal CLFlow()
        {
            _deserializer = new RegexOptionsDeserializer();
            _fallbackFunc = (error) => { };
        }

        public CLFlow UseFallback(Action<ErrorCode> fallback)
        {
            _fallbackFunc = fallback ?? throw new ArgumentNullException(nameof(fallback));
            return this;
        }

        public CLFlow UseMapping(Mapping mapping)
        {
            _config = mapping ?? throw new ArgumentNullException(nameof(mapping));
            return this;
        }

        public CLFlow UseServiceProvider(IServiceProvider provider)
        {
            _serviceProvider = provider ?? throw new ArgumentOutOfRangeException(nameof(provider));
            return this;
        }

        public CLFlow UseDeserializer(IOptionsDeserializer deserializer)
        {
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            return this;
        }

        public int Run(string[] args)
        {
            if (_config == null && _serviceProvider == null)
            {
                throw new InvalidOperationException($"{nameof(Mapping)} need to be provided either with {nameof(UseMapping)} method or via {nameof(IServiceProvider)}.");
            }

            var config = _config ?? _serviceProvider.GetService<Mapping>() ?? throw new InvalidOperationException();

            if (!args.Any())
            {
                //TODO Add default handling here
                return Fallback(ErrorCode.UnspecifiedCommand);
            }

            var mappingEntry = config.Entries.FirstOrDefault(it =>
                 Attribute.IsDefined(it.CommandType, typeof(CommandAttribute)) &&
                 (Attribute.GetCustomAttribute(it.CommandType, typeof(CommandAttribute)) as CommandAttribute).Name == args[0]);

            if (mappingEntry == null)
            {
                return Fallback(ErrorCode.UnrecognizedCommand);
            }

            object command;

            try
            {
                command = _deserializer.Deserialize(mappingEntry.CommandType, args.Skip(1).ToArray());
            }
            catch (DeserializationException ex)
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

        public static CLFlow Create()
        {
            return new CLFlow();
        }
    }

    public static class ServiceCollectionExtentions
    {
        public static IServiceCollection ConfigureCLFlow(this IServiceCollection services, Action<Mapping> setup)
        {
            if (services == null)
            {
                throw new ArgumentNullException($"{nameof(services)} cannot be null.");
            }

            if (setup == null)
            {
                throw new ArgumentNullException($"{nameof(setup)} cannot be null.");
            }

            Mapping mapping = new Mapping();
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
