using System;

namespace inausoft.netCLI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandNameAttribute : Attribute
    {
        public string CommandName { get; }

        public CommandNameAttribute(string commandName)
        {
            CommandName = commandName;
        }
    }
}
