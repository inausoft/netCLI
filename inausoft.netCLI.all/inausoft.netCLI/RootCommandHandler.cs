using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace inausoft.netCLI
{
    /// <summary>
    /// Application entry point, responsible for parsing command line arguments and running related <see cref="ICommandHandler"/>.
    /// </summary>
    public class RootCommandHandler
    {
        private static string commandPattern = @"^(\w+)( --\S+\s?\w\S*)*$";

        //pattern for fetching 'option value' pairs from command line string.
        private static string optionsPattern = @"--(\S+)\s?(\w\S*)*";

        /// <summary>
        /// Returns list of all available <see cref="ICommandHandler"/>.
        /// </summary>
        internal List<ICommandHandler> CommandHandlers { get; }

        /// <summary>
        /// Initialize instance of a <see cref="RootCommandHandler"/> with a defined set of <see cref="ICommandHandler"/>.
        /// </summary>
        /// <param name="commandHandlers"></param>
        public RootCommandHandler(IEnumerable<ICommandHandler> commandHandlers)
        {
            CommandHandlers = commandHandlers.ToList();
        }

        /// <summary>
        /// Runs <see cref="ICommandHandler"/> for command specified in args./>
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns></returns>
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
                //TODO: I think that this part is overcomplicated. See MateuszCommand Folder
                return commandHandler.Run(CreateCommandFromExpression(commandHandler.GetCommandType(), fullExpresion));
            }

            return 0;
        }

        //Creates a command object from command line arguments.
        private object CreateCommandFromExpression(Type commandType, string expression)
        {
            var command = Activator.CreateInstance(commandType);

            var options = new Regex(optionsPattern).Matches(expression);

            foreach (Match option in options)
            {
                var property = commandType.GetProperties().FirstOrDefault(it => Attribute.IsDefined(it, typeof(OptionAttribute))
                                                        && (Attribute.GetCustomAttribute(it, typeof(OptionAttribute)) as OptionAttribute).Name == option.Groups[1].Value);

                //if there is no value for an option. Ex. 'move --force' as 'opposed to --force true'
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
