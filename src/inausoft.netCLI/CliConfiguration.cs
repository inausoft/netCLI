using inausoft.netCLI.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace inausoft.netCLI
{
    public static class CliConfigurationExtentions
    {
        /// <summary>
        /// Setups CLI flow by adding and configuring <see cref="RootCommandHandler"/>.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="SetupHelpCommand"></param>
        /// <returns></returns>
        public static IServiceCollection AddCLI(this IServiceCollection services, bool SetupHelpCommand = false)
        {
            if(services == null)
            {
                throw new ArgumentNullException($"{nameof(services)} cannot be null.");
            }

            if (SetupHelpCommand)
            {
                return services.AddSingleton<RootCommandHandler>(provider => 
                {
                    var rootCommandHandler = new RootCommandHandler(provider.GetServices<ICommandHandler>());
                    rootCommandHandler.CommandHandlers.Add(new HelpCommandHandler(rootCommandHandler.CommandHandlers,
                                                            provider.GetRequiredService<ILogger<HelpCommandHandler>>()));

                    return rootCommandHandler;
                });
            }

            return services.AddSingleton<RootCommandHandler>();
        }

        /// <summary>
        /// Runs <see cref="ICommandHandler"/> for command specified in args./>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="args">Application's command line arguments.</param>
        /// <returns></returns>
        public static int RunCLI(this IServiceProvider serviceProvider, string[] args)
        {
            var rootCommandHandler = serviceProvider.GetService<RootCommandHandler>();

            if(rootCommandHandler == null)
            {
                throw new InvalidOperationException($"{nameof(RootCommandHandler)} was not registered. Run '{nameof(AddCLI)}' first.");
            }

            return rootCommandHandler.Run(args);
        }
    }
}
