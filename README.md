[![GitHub issues open](https://img.shields.io/github/issues/inausoft/netCLI.svg?style=flat-square)]()
[![NuGet version (inausoft.netCLI)](https://img.shields.io/nuget/v/inausoft.netCLI.svg?style=flat-square)](https://www.nuget.org/packages/inausoft.netCLI/)


# netCLI

![Logo](src/inausoft.netCLI/assets/netCLI.png)

netCLI is a lightweight library meant to facilitate command line arguments parsing and flow control over .net CLI applications.

## Quick Start:

```cs
ICommandHandler[] handlers = new ICommandHandler[] { 
    new MoveCommandHandler() 
};

var config = new CliConfiguration().Map<MoveCommand, MoveCommandHandler>();

return netCLI.RunCLI(config, args, handlers);

```


## Using dependency injection:

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
            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
            });
            services.AddCLI(config => {
                config.Map<MoveCommand, MoveCommandHandler>()
                      .MapHelpCommand();
            });

            var provider = services.BuildServiceProvider();

            return provider.RunCLI(args);
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

# Contributors

- Iwanowski, Marcin
- Kaczor, Mateusz (matiduck)
