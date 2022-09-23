[![GitHub issues open](https://img.shields.io/github/issues/inausoft/netCLI.svg?style=flat-square)]()
[![NuGet version (inausoft.netCLI)](https://img.shields.io/nuget/v/inausoft.netCLI.svg?style=flat-square)](https://www.nuget.org/packages/inausoft.netCLI/)


# netCLI

![Logo](src/inausoft.netCLI/assets/netCLI.png)

netCLI is a lightweight library meant to facilitate command line arguments parsing and flow control over .net CLI applications.

## Supported scenarios:
`> command --option <optionValue>`  
`> command -o <optionValue>`  
`> command --option`  
`> command -o`

# Getting Started

- Create .net 6 Console Application
- Install `inausoft.netCLI` via [NuGet](https://www.nuget.org/packages/inausoft.netCLI/)
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
            var mapping = new CommandMapping().Map<MoveCommand>(new MoveCommandHandler());

            return CLIFlow.Create().UseMapping(mapping)
                                  .Run(args);
        }
    }

    [Command("move", "Moves files between locations.")]
    public class MoveCommand
    {
        [Option("force", "Indicates whether or not files should be overwritten.", IsOptional = true)]
        public bool IsForce { get; set; }

        [Option("from|f", "Current files locations.")]
        public string PathFrom { get; set; }

        [Option("to|t", "Destination path for the files.")]
        public bool PathTo { get; set; }
    }

    class MoveCommandHandler : CommandHandler<MoveCommand>
    {
        public override int Run(MoveCommand options)
        {
            Console.WriteLine($"Files moved from {options.PathFrom} to {options.PathTo}");

            return 0;
        }
    }
}

```


## Using dependency injection:

```cs
...

namespace netCLIConsoleApp
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

If you like to join or contribute please see our [Contributing guidelines](CONTRIBUTING.md)
