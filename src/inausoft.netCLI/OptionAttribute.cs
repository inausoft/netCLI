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
        /// Gets the name of an option.
        /// </summary>
        public string Name { get; }

        public string HelpDescription { get; }

        public bool IsOptional { get; set; }

        public OptionAttribute(string name, string helpDescription = "")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));

            HelpDescription = helpDescription ?? throw new ArgumentNullException(nameof(helpDescription));
        }
    }
}
