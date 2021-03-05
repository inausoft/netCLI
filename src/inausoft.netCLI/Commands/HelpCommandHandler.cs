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
        private readonly Mapping _configuration;

        private readonly ILogger<HelpCommandHandler> _logger;

        public HelpCommandHandler(Mapping configuration, ILogger<HelpCommandHandler> logger)
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

                foreach (var commandInfo in _configuration.CommandInfos)
                {
                    message.AppendLine(string.Format("{0, -15} {1}", commandInfo.Command.Name, commandInfo.Command.HelpDescription));
                }

                message.AppendLine();
                message.AppendLine($"Run 'help --command <commandName>' for detailed information.");

                _logger.LogInformation(message.ToString());
            }
            else
            {
                var commandInfo = _configuration.CommandInfos.FirstOrDefault(it => it.Command.Name == command.SpecifiedCommandName);

                if (commandInfo == null)
                {
                    throw new InvalidCommandException(command.SpecifiedCommandName, $"Command {command.SpecifiedCommandName} is invalid.");
                }

                StringBuilder message = new StringBuilder($"Usage: {command.SpecifiedCommandName}");

                if(commandInfo.Options.Any())
                {
                    message.AppendLine(" [OPTIONS]");
                    message.AppendLine();
                    message.AppendLine("options:");

                    foreach (var option in commandInfo.Options)
                    {
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
