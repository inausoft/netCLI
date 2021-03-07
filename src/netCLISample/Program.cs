using Microsoft.Extensions.DependencyInjection;
using System;
using inausoft.netCLI;
using Microsoft.Extensions.Logging;
using inausoft.netCLI.Commands;

namespace netCLISample
{
    class Program
    {
        static ILogger<Program> logger;

        static int Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Debug);
                builder.AddConsole();
            });
            services.ConfigureCLFlow(config =>
            {
                config.Map<MoveCommand, MoveCommandHandler>()
                      .Map<HelpCommand, HelpCommandHandler>();

            });

            using( var provider = services.BuildServiceProvider())
            {
                logger = provider.GetRequiredService<ILogger<Program>>();

                return CLFlow.Create().UseServiceProvider(provider)
                                      .UseFallback(ErrorHandling)
                                      .Run(args);
            }
        }

        public static void ErrorHandling(ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.RequiredOptionMissing:
                    logger.LogError("Could not execute command. Some required options were missing.");
                    break;
                default:
                    logger.LogError("Could not execute command due to some general error.");
                    break;
            }
            logger.LogInformation("Try `help --command <command>` to see more details.");
        }
    }

    [Command("move", "Moves files between locations.")]
    public class MoveCommand
    {
        [Option("force", "Indicates weather or not files should be overwritten.")]
        public bool IsForce { get; set; }
    }

    public class MoveCommandHandler : CommandHandler<MoveCommand>
    {
        public override int Run(MoveCommand options)
        {
            Console.WriteLine("bla");
            return 0;
        }
    }
}
