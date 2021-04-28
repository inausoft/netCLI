using System;

namespace inausoft.netCLI
{
    /// <summary>
    /// Marks class as a verb command. Related properties should be mark using <see cref="OptionAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the command.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the command.
        /// </summary>
        public string HelpDescription { get; }

        /// <summary>
        /// Initializes a new instance of <see cref="CommandAttribute"/> with specified name and description.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="helpDescription"></param>
        public CommandAttribute(string name, string helpDescription = "")
        {
            if(string.IsNullOrWhiteSpace(name) || name.StartsWith("-"))
            {
                throw new ArgumentException(nameof(name));
            }

            Name = name;

            HelpDescription = helpDescription ?? throw new ArgumentNullException(nameof(helpDescription));
        }
    }
}
