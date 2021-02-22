# netCLI

netCLI is a lightwaight library ment for faciliate command line arguments parsing and flow control over .net CLI applications.

C# Quick Start:

```cs
using inausoft.netCLI;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddSingleton<ICommandHandler, MoveCommandHandler>();
            services.AddCLI();

            var provider = services.BuildServiceProvider();
            var rootCommandHandler = provider.GetRequiredService<RootCommandHandler>();

            rootCommandHandler.Run(args);
        }
    }

    [Command("move", "Moves files from one path to another.")]
    class MoveCommand
    {
        [Option("pathFrom")]
        public string From { get; set; }

        [Option("pathTo")]
        public string To { get; set; }

        [Option("force")]
        public bool IsForce { get; set; }
    }

    class MoveCommandHandler : CommandHandler<MoveCommand>
    {
        public override int Run(MoveCommand options)
        {
            Console.WriteLine($"Files moved from {options.From} to {options.To}");
            
            return 0;
        }
    }
}

```