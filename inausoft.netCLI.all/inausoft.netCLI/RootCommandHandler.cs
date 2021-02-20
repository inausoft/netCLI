using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace inausoft.netCLI
{
    public class RootCommandHandler
    {
        private static string commandPattern = @"^(\w+)( --\S+\s?\S*)*$";

        private static string optionsPattern = @"--(\S+)\s?(\w\S+)*";

        private IEnumerable<CommandHandler> _commandHandlers;

        public RootCommandHandler(IEnumerable<CommandHandler> commandHandlers)
        {
            _commandHandlers = commandHandlers;
        }

        public int Run(string[] args)
        {
            var fullExpresion = string.Join(" ", args);

            var match = new Regex(commandPattern).Match(fullExpresion);

            if (match.Success)
            {
                var commandName = match.Groups[1].Value;

                var commandHandler = _commandHandlers.FirstOrDefault(it =>
                    Attribute.IsDefined(it.GetOptionsType(), typeof(CommandNameAttribute)) &&
                    (Attribute.GetCustomAttribute(it.GetOptionsType(), typeof(CommandNameAttribute)) as CommandNameAttribute).CommandName == commandName);

                if (commandHandler == null)
                {
                    throw new Exception($"Command {commandName} was not found");
                }

                var commandType = commandHandler.GetOptionsType();

                var command = Activator.CreateInstance(commandType);

                var options = new Regex(optionsPattern).Matches(fullExpresion);

                foreach (Match option in options)
                {
                    var property = commandType.GetProperties().FirstOrDefault(it => Attribute.IsDefined(it, typeof(OptionAtrribute))
                                                            && (Attribute.GetCustomAttribute(it, typeof(OptionAtrribute)) as OptionAtrribute).Name == option.Groups[1].Value);

                    if (string.IsNullOrEmpty(option.Groups[2].Value))
                    {
                        property.SetMethod.Invoke(command, new object[] { true });
                    }
                    else
                    {
                        property.SetMethod.Invoke(command, new object[] { Convert.ChangeType(option.Groups[2].Value, property.PropertyType) });
                    }

                }

                return commandHandler.Run(command);
            }

            return 0;
        }

    }
}
