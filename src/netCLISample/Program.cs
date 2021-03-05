using Microsoft.Extensions.DependencyInjection;
using System;
using inausoft.netCLI;
using Microsoft.Extensions.Logging;
using inausoft.netCLI.Commands;

namespace netCLISample
{
    class Program
    {
        static int Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddProvider(new MyConsoleLoggerProvider());
            });
            services.ConfigureCLFlow(config =>
            {
                config.Map<MoveCommand, MoveCommandHandler>()
                      .Map<HelpCommand, HelpCommandHandler>();

            });

            var provider = services.BuildServiceProvider();

            return CLFlow.Create().UseServiceProvider(provider)
                                  .UseFallback(ErrorHandling)
                                  .Run(args);
        }

        public static void ErrorHandling(ErrorCode errorCode)
        {
            Console.WriteLine($"Bla error {errorCode}.");
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
            return 1;
        }
    }
}
