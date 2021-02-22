using System;

namespace inausoft.netCLI
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAttribute : Attribute
    {
        public string Name { get; }

        public string HelpDescription { get; }

        public OptionAttribute(string name)
        {
            Name = name;
        }
    }
}
