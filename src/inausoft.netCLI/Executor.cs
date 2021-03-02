using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace inausoft.netCLI
{
    public static class Executor
    {
        /// <summary>
        /// Setups CLI flow by adding and configuring <see cref="RootCommandHandler"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="SetupHelpCommand"></param>
        /// <returns></returns>
        public static IServiceCollection AddCLI(this IServiceCollection services, Action<CLIConfiguration> setup)
        {
            if (services == null)
            {
                throw new ArgumentNullException($"{nameof(services)} cannot be null.");
            }

            CLIConfiguration configuration = new CLIConfiguration();

            setup(configuration);

            services.AddSingleton(configuration);

            configuration._commandMap.Values.ToList().ForEach(it =>
            {
                services.AddSingleton(it);
            });

            return services;
        }

        /// <summary>
        /// Runs <see cref="ICommandHandler"/> for command specified in args./>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="args">Application's command line arguments.</param>
        /// <returns></returns>
        public static int RunCLI(this IServiceProvider serviceProvider, string[] args)
        {
            var config = serviceProvider.GetService<CLIConfiguration>();

            if (config == null)
            {
                throw new InvalidOperationException($"{nameof(CLIConfiguration)} was not registered. Run '{nameof(AddCLI)}' first.");
            }

            if (!args.Any())
            {
                //TODO Add default handling here
                throw new InvalidCommandException($"Command was not specified.");
            }

            var commandType = config._commandMap.Keys.FirstOrDefault(it =>
                Attribute.IsDefined(it, typeof(CommandAttribute)) &&
                (Attribute.GetCustomAttribute(it, typeof(CommandAttribute)) as CommandAttribute).Name == args[0]);

            if (commandType == null)
            {
                throw new InvalidCommandException(args[0], $"Command {args[0]} was not mapped.");
            }

            var command = config.Deserializer.Deserialize(commandType, args.Skip(1).ToArray());

            var handler = serviceProvider.GetRequiredService(config._commandMap[command.GetType()]) as ICommandHandler;

            return handler.Run(command);
        }

        public static int RunCLI(CLIConfiguration config, string[] args, params ICommandHandler[] handlers)
        {
            if (config == null)
            {
                throw new InvalidOperationException($"{nameof(CLIConfiguration)} was not registered. Run '{nameof(AddCLI)}' first.");
            }

            if (!args.Any())
            {
                //TODO Add default handling here
                throw new InvalidCommandException($"Command was not specified.");
            }

            var commandType = config._commandMap.Keys.FirstOrDefault(it =>
                Attribute.IsDefined(it, typeof(CommandAttribute)) &&
                (Attribute.GetCustomAttribute(it, typeof(CommandAttribute)) as CommandAttribute).Name == args[0]);

            if (commandType == null)
            {
                throw new InvalidCommandException(args[0], $"Command {args[0]} was not mapped.");
            }

            var command = config.Deserializer.Deserialize(commandType, args.Skip(1).ToArray());

            var handler = handlers.First(it => it.GetType() == config._commandMap[command.GetType()]) as ICommandHandler;

            return handler.Run(command);
        }
    }
}
