using inausoft.netCLI.Deserialization;
using System;

namespace inausoft.netCLI
{
    public class CLIFlowBuilder
    {
        private CLIFlow cLIFlow;


        private IServiceProvider _serviceProvider;

        private CommandMapping _mapping;

        private ICommandDeserializer _deserializer;

        private Func<ErrorCode, int> _fallbackFunc;

        public CLIFlowBuilder()
        {
            _deserializer = new LogicalCommandDeserializer();
            _fallbackFunc = (error) => { return (int)error; };
        }

        public CLIFlowBuilder UseFallback(Func<ErrorCode, int> fallback)
        {
            _fallbackFunc = fallback ?? throw new ArgumentNullException(nameof(fallback));
            return this;
        }

        /// <summary>
        /// Instructs to explicitly use specified <see cref="CommandMapping"/>.
        /// </summary>
        /// <param name="mapping"></param>
        /// <returns></returns>
        public CLIFlowBuilder UseMapping(CommandMapping mapping)
        {
            _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
            return this;
        }

        /// <summary>
        /// Instructs to use <see cref="IServiceProvider"/> to create instances of mapped <see cref="ICommandHandler"/>.
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public CLIFlowBuilder UseServiceProvider(IServiceProvider provider)
        {
            _serviceProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            return this;
        }

        /// <summary>
        /// Instructs to use supplied <see cref="ICommandDeserializer"/> instead of default implementation for the purpose of deserializing command arguments.
        /// </summary>
        /// <param name="deserializer"></param>
        /// <returns></returns>
        public CLIFlowBuilder UseDeserializer(ICommandDeserializer deserializer)
        {
            _deserializer = deserializer ?? throw new ArgumentNullException(nameof(deserializer));
            return this;
        }


        public CLIFlow Build()
        {
            Validate();
            return new CLIFlow()
            {
                FallbackFunc = this._fallbackFunc,
                Deserializer = this._deserializer,
                Mapping = this._mapping,
                ServiceProvider = this._serviceProvider,
            };
        }

        private void Validate()
        {
            if (_mapping == null && _serviceProvider == null)
            {
                throw new InvalidOperationException($"{nameof(CommandMapping)} need to be provided either with {nameof(UseMapping)} method or via {nameof(IServiceProvider)}.");
            }

        }
    }
}
