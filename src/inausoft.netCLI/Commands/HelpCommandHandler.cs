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
        private readonly IEnumerable<ICommandHandler> _commandHandlers;

        private readonly ILogger<HelpCommandHandler> _logger;

        public HelpCommandHandler(IEnumerable<ICommandHandler> commandHandlers, ILogger<HelpCommandHandler> logger)
        {
            _commandHandlers = commandHandlers ?? throw new ArgumentNullException(nameof(commandHandlers));

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
                foreach (var commandHandler in _commandHandlers)
                {
                    var commandType = commandHandler.GetCommandType();

                    if (Attribute.IsDefined(commandType, typeof(CommandAttribute)))
                    {
                        CommandAttribute attribute = Attribute.GetCustomAttribute(commandType, typeof(CommandAttribute)) as CommandAttribute;

                        _logger.LogInformation($"{attribute.Name} \t\t {attribute.HelpDescription}");
                    }
                }
            }
            else
            {
                var commandHandler = _commandHandlers.FirstOrDefault(it =>
                    Attribute.IsDefined(it.GetCommandType(), typeof(CommandAttribute)) &&
                    (Attribute.GetCustomAttribute(it.GetCommandType(), typeof(CommandAttribute)) as CommandAttribute).Name == command.SpecifiedCommandName);

                if (commandHandler == null)
                {
                    throw new InvalidCommandException(command.SpecifiedCommandName, $"Command {command.SpecifiedCommandName} is invalid.");
                }

                StringBuilder message = new StringBuilder(command.SpecifiedCommandName);

                var properties = commandHandler.GetCommandType().GetProperties().Where(it => Attribute.IsDefined(it, typeof(OptionAttribute)));

                foreach(var property in properties)
                {
                    var option = (Attribute.GetCustomAttribute(property, typeof(OptionAttribute)) as OptionAttribute).Name;
                    message.Append($" --{option} <{option.ToUpper()}>");
                }

                _logger.LogInformation(message.ToString());
            }
            
            return 0;
        }
    }
}
