using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace inausoft.netCLI
{
    public class RootCommandHandler
    {
        private static string commandPattern = @"^(\w+)( --\S+\s?\S*)*$";

        private static string optionsPattern = @"--(\S+)\s?(\w\S+)*";

        internal List<ICommandHandler> CommandHandlers { get; }

        public RootCommandHandler(IEnumerable<ICommandHandler> commandHandlers)
        {
            CommandHandlers = commandHandlers.ToList();
        }

        public int Run(string[] args)
        {
            var fullExpresion = string.Join(" ", args);

            var match = new Regex(commandPattern).Match(fullExpresion);

            if (match.Success)
            {
                var commandName = match.Groups[1].Value;

                var commandHandler = CommandHandlers.FirstOrDefault(it =>
                    Attribute.IsDefined(it.GetCommandType(), typeof(CommandAttribute)) &&
                    (Attribute.GetCustomAttribute(it.GetCommandType(), typeof(CommandAttribute)) as CommandAttribute).Name == commandName);

                if (commandHandler == null)
                {
                    throw new InvalidCommandException(commandName, $"Command {commandName} is invalid.");
                }

                return commandHandler.Run(CreateCommandFromExpression(commandHandler.GetCommandType(), fullExpresion));
            }

            return 0;
        }

        private object CreateCommandFromExpression(Type commandType, string expression)
        {
            var command = Activator.CreateInstance(commandType);

            var options = new Regex(optionsPattern).Matches(expression);

            foreach (Match option in options)
            {
                var property = commandType.GetProperties().FirstOrDefault(it => Attribute.IsDefined(it, typeof(OptionAttribute))
                                                        && (Attribute.GetCustomAttribute(it, typeof(OptionAttribute)) as OptionAttribute).Name == option.Groups[1].Value);

                if (string.IsNullOrEmpty(option.Groups[2].Value))
                {
                    property.SetMethod.Invoke(command, new object[] { true });
                }
                else
                {
                    property.SetMethod.Invoke(command, new object[] { Convert.ChangeType(option.Groups[2].Value, property.PropertyType) });
                }
            }

            return command;
        }
    }
}
