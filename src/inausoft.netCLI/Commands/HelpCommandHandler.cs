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
        private readonly CliConfiguration _configuration;

        private readonly ILogger<HelpCommandHandler> _logger;

        public HelpCommandHandler(CliConfiguration configuration, ILogger<HelpCommandHandler> logger)
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
                foreach (var commandType in _configuration.CommandTypes)
                {
                    if (Attribute.IsDefined(commandType, typeof(CommandAttribute)))
                    {
                        CommandAttribute attribute = Attribute.GetCustomAttribute(commandType, typeof(CommandAttribute)) as CommandAttribute;

                        _logger.LogInformation($"{attribute.Name} \t\t {attribute.HelpDescription}");
                    }
                }
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

                StringBuilder message = new StringBuilder($"{command.SpecifiedCommandName}");

                var properties = commandType.GetProperties().Where(it => Attribute.IsDefined(it, typeof(OptionAttribute)));

                if(properties != null)
                {
                    message.AppendLine(" [OPTIONS]");
                    message.AppendLine("options:");
                }

                foreach(var property in properties)
                {
                    var option = (Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute);
                    message.AppendLine($" --{option.Name} \t\t {option.HelpDescription}");
                }

                _logger.LogInformation(message.ToString());
            }
            
            return 0;
        }
    }
}
