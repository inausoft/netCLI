[![GitHub issues open](https://img.shields.io/github/issues/inausoft/netCLI.svg?style=flat-square)]()
[![NuGet version (inausoft.netCLI)](https://img.shields.io/nuget/v/inausoft.netCLI.svg?style=flat-square)](https://www.nuget.org/packages/inausoft.netCLI/)


# netCLI

![Logo](src/inausoft.netCLI/assets/netCLI.png)

netCLI is a lightweight library meant to facilitate command line arguments parsing and flow control over .net CLI applications.

## Supported scenarios:
`> command --option <optionValue>`  
`> command --option`

# Getting Started

- Create .net Core Console Application
- Install netCLI via [NuGet](https://www.nuget.org/packages/inausoft.netCLI/)
- Adjust Program.cs with the following code:


```cs
using inausoft.netCLI;
using System;

namespace netCLIConsoleApp
{
    class Program
    {
        static int Main(string[] args)
        {
            var mapping = new CLIConfiguration().Map<MoveCommand>(new MoveCommandHandler());

            return CLFlow.Create().UseMapping(mapping)
                                  .Run(args);
        }
    }

    [Command("move", "Moves files from one path to another.")]
    class MoveCommand
    {
        [Option("pathFrom", "Current files locations.")]
        public string From { get; set; }

        [Option("pathTo", "Destination path for the files.")]
        public string To { get; set; }

        [Option("force", "Overrides if files exists.")]
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


## Using dependency injection:

```cs
...

namespace ConsoleApp1
{
    class Program
    {
        static int Main(string[] args)
        {
            var services = new ServiceCollection();
            services.ConfigureCLFlow(mapping =>
            {
                mapping.Map<MoveCommand, MoveCommandHandler>()
                       .Map<HelpCommand, HelpCommandHandler>();

            });

            using( var provider = services.BuildServiceProvider())
            {
                return CLFlow.Create().UseServiceProvider(provider)
                                      .Run(args);
            }
        }
    }
}
...

```

# Contributors

- Iwanowski, Marcin
- Kaczor, Mateusz (matiduck)
