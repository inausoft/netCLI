using System;

namespace inausoft.netCLI
{
    [AttributeUsage(AttributeTargets.Property)]
    public class OptionAtrribute : Attribute
    {
        public string Name { get; }

        public OptionAtrribute(string name)
        {
            Name = name;
        }
    }
}
