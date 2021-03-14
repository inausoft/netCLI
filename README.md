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

    [Command("move", "Moves files between locations.")]
    public class MoveCommand
    {
        [Option("force", "Indicates weather or not files should be overwritten.", IsOptional = true)]
        public bool IsForce { get; set; }

        [Option("from", "Current files locations.")]
        public string PathFrom { get; set; }

        [Option("to", "Destination path for the files.")]
        public bool PathTo { get; set; }
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

            using (ServiceProvider provider = services.BuildServiceProvider())
            {
                logger = provider.GetRequiredService<ILogger<Program>>();

                return CLFlow.Create().UseServiceProvider(provider)
                                      .UseFallback(ErrorHandling)
                                      .Run(args);
            }
        }
    }
}
...

```

# Contributing
## Full list of our contributors (in order of making first contribution):
- Iwanowski, Marcin (MarcinIN)
- Kaczor, Mateusz (matiduck)
- Marlewski, Jan (jmarlew)

If you like to join or contribute please see our [Contributing guidelines](CONTRIBUTING.md)
