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
        /// Gets the full name of the option.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the short name of the option.
        /// </summary>
        public string ShortName { get; private set; }

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
        /// <param name="naming"></param>
        /// <param name="helpDescription"></param>
        public OptionAttribute(string naming, string helpDescription = "")
        {
            if (naming == null)
            {
                throw new ArgumentNullException(nameof(naming));
            }

            var names = naming.Split('|');

            if(names.Length > 2 || names.Length == 0)
            {
                throw new ArgumentException(nameof(naming));
            }

            Name = names[0];

            if(names.Length == 2)
            {
                ShortName = names[1];
            }

            HelpDescription = helpDescription ?? throw new ArgumentNullException(nameof(helpDescription));
        }
    }
}
