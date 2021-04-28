using System;

namespace inausoft.netCLI
{
    /// <summary>
    /// Marks property as an option of a verb command. Class should be marked using <see cref="CommandAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the option.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the option.
        /// </summary>
        public string HelpDescription { get; }

        /// <summary>
        /// Gets or sets a value indicating whether specified option is optional or required.
        /// </summary>
        public bool IsOptional { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="OptionAttribute"/> with specified name and description.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="helpDescription"></param>
        public OptionAttribute(string name, string helpDescription = "")
        {
            if (string.IsNullOrWhiteSpace(name) || name.StartsWith("-"))
            {
                throw new ArgumentException(nameof(name));
            }

            Name = name;

            HelpDescription = helpDescription ?? throw new ArgumentNullException(nameof(helpDescription));
        }
    }
}
