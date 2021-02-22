using System;

namespace inausoft.netCLI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; }

        public string HelpDescription { get; }

        public CommandAttribute(string name, string helpDescription = "")
        {
            Name = name ?? throw new ArgumentException(nameof(name));

            HelpDescription = helpDescription;
        }
    }
}
