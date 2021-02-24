using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static inausoft.netCLI.MateuszCommands.MateuszCommandHandler;

namespace inausoft.netCLI.MateuszCommands
{
    public class MateuszRootCommandHandler
    {

        private static string commandPattern = @"^(\w+)( --\S+\s?\w\S*)*$";

        //pattern for fetching 'option value' pairs from command line string.
        private static string optionsPattern = @"--(\S+)\s?(\w\S*)*";

        public IEnumerable<IMateuszCommandHandler<IMateuszCommand>> CommandHandlers;

        public MateuszRootCommandHandler(IEnumerable<IMateuszCommandHandler<IMateuszCommand>> mateuszCommands)
        {
            CommandHandlers = mateuszCommands;
        }
        public int Run(string[] args)
        {
            var fullExpresion = string.Join(" ", args);

            var match = new Regex(commandPattern).Match(fullExpresion);

            if (match.Success)
            {
                //Part responsible for command
                var commandName = match.Groups[1].Value;

                //TODO: registration and found appopriate commandHandler
                var commandHandler = CommandHandlers.First();

                if (commandHandler == null)
                {
                    throw new InvalidCommandException(commandName, $"Command {commandName} is invalid.");
                }

                //Part responsible for arguments
                var arguments = new Regex(optionsPattern).Matches(fullExpresion);
                foreach (Match argument in arguments)
                {
                    var property = commandHandler.GetCommand().GetType().GetProperties().FirstOrDefault(it => Attribute.IsDefined(it, typeof(OptionAttribute))
                                                            && (Attribute.GetCustomAttribute(it, typeof(OptionAttribute)) as OptionAttribute).Name == argument.Groups[1].Value);

                    //if there is no value for an option. Ex. 'move --force' as 'opposed to --force true'
                    if (string.IsNullOrEmpty(argument.Groups[2].Value))
                    {
                        property.SetMethod.Invoke(commandHandler.GetCommand(), new object[] { true });
                    }
                    else
                    {
                        property.SetMethod.Invoke(commandHandler.GetCommand(), new object[] { Convert.ChangeType(argument.Groups[2].Value, property.PropertyType) });
                    }
                }

                return commandHandler.Run();
            }

            return 0;
        }

    }
}