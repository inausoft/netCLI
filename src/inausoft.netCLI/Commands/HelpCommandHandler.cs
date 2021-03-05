using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace inausoft.netCLI.Commands
{
    /// <summary>
    /// Deafult handler for 'help' command.
    /// </summary>
    public class HelpCommandHandler : CommandHandler<HelpCommand>
    {
        private readonly CLIConfiguration2 _configuration;

        private readonly ILogger<HelpCommandHandler> _logger;

        public HelpCommandHandler(CLIConfiguration2 configuration, ILogger<HelpCommandHandler> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override int Run(HelpCommand command)
        {
            if(command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (string.IsNullOrEmpty(command.SpecifiedCommandName))
            {
                StringBuilder message = new StringBuilder();
                message.AppendLine("Available commands:");
                message.AppendLine();

                foreach (var commandType in _configuration.CommandTypes)
                {
                    if (Attribute.IsDefined(commandType, typeof(CommandAttribute)))
                    {
                        CommandAttribute commadType = Attribute.GetCustomAttribute(commandType, typeof(CommandAttribute)) as CommandAttribute;

                        message.AppendLine(string.Format("{0, -15} {1}", commadType.Name, commadType.HelpDescription));
                    }
                }

                message.AppendLine();
                message.AppendLine($"Run 'help --command <commandName>' for detailed information.");

                _logger.LogInformation(message.ToString());
            }
            else
            {
                var commandType = _configuration.CommandTypes.FirstOrDefault(it =>
                    Attribute.IsDefined(it, typeof(CommandAttribute)) &&
                    (Attribute.GetCustomAttribute(it, typeof(CommandAttribute)) as CommandAttribute).Name == command.SpecifiedCommandName);

                if (commandType == null)
                {
                    throw new InvalidCommandException(command.SpecifiedCommandName, $"Command {command.SpecifiedCommandName} is invalid.");
                }

                StringBuilder message = new StringBuilder($"Usage: {command.SpecifiedCommandName}");

                var properties = commandType.GetProperties().Where(it => Attribute.IsDefined(it, typeof(OptionAttribute)));

                if(properties.Any())
                {
                    message.AppendLine(" [OPTIONS]");
                    message.AppendLine();
                    message.AppendLine("options:");

                    foreach (var property in properties)
                    {
                        var option = (Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute);
                        message.AppendLine(string.Format("--{0, -15} {1}", option.Name, option.HelpDescription));
                    }
                }

                message.AppendLine();
                _logger.LogInformation(message.ToString());
            }
            
            return 0;
        }
    }
}
