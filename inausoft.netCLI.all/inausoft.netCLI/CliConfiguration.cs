using inausoft.netCLI.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace inausoft.netCLI
{
    public static class CliConfigurationExtentions
    {
        public static IServiceCollection AddCLI(this IServiceCollection services, bool SetupHelpCommand = false)
        {
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
    }
}
