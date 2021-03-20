using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Text;

namespace inausoft.netCLI.Commands
{
    /// <summary>
    /// Default handler for <see cref="HelpCommand"/>, that prints information about mapped commands.
    /// </summary>
    public class HelpCommandHandler : CommandHandler<HelpCommand>
    {
        private readonly CommandMapping _mapping;

        private readonly ILogger<HelpCommandHandler> _logger;

        /// <summary>
        /// Initializes new instance of <see cref="HelpCommandHandler"/> with the specified <see cref="CommandMapping"/>.
        /// </summary>
        /// <param name="mapping"></param>
        /// <param name="logger"></param>
        public HelpCommandHandler(CommandMapping mapping, ILogger<HelpCommandHandler> logger)
        {
            _mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override int Run(HelpCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (string.IsNullOrWhiteSpace(command.SpecifiedCommandName))
            {
                //List all available commands.
                StringBuilder message = new StringBuilder();
                message.AppendLine("Available commands:");
                message.AppendLine();

                foreach (var commandInfo in _mapping.CommandInfos.OrderBy(it => it.Command.Name))
                {
                    message.AppendLine(string.Format(" {0, -15} {1}", commandInfo.Command.Name, commandInfo.Command.HelpDescription));
                }

                message.AppendLine();
                message.AppendLine($"Run 'help --command <commandName>' for detailed information.");

                _logger.LogInformation(message.ToString());
            }
            else
            {
                //Display detailed help for one specified command.
                var commandInfo = _mapping.CommandInfos.FirstOrDefault(it => it.Command.Name == command.SpecifiedCommandName);

                if (commandInfo == null)
                {
                    _logger.LogInformation($"Command : {command.SpecifiedCommandName} is not recognized.");
                    return 1;
                }

                StringBuilder message = new StringBuilder();
                message.AppendLine($"{command.SpecifiedCommandName} - {commandInfo.Command.HelpDescription}");
                message.Append($"Usage: {command.SpecifiedCommandName}");

                if (commandInfo.Options.Any())
                {
                    message.AppendLine(" [OPTIONS]");
                    message.AppendLine();
                    message.AppendLine("options:");

                    foreach (var option in commandInfo.Options.OrderBy(it => it.Name))
                    {
                        message.AppendLine(string.Format("--{0, -15} {1} {2}", option.Name, option.HelpDescription, RenderOptional(option.IsOptional)));
                    }
                }

                message.AppendLine();
                _logger.LogInformation(message.ToString());
            }

            return 0;
        }

        private string RenderOptional(bool IsOptional)
        {
            if (IsOptional)
            {
                return "[OPTIONAL]";
            }
            else
            {
                return "";
            }
        }
    }
}
