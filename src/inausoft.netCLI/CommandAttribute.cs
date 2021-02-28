using System;

namespace inausoft.netCLI
{
    /// <summary>
    /// Marks class as a verb command. Related properties should be mark using <see cref="OptionAttribute"/>
    /// </summary>
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
