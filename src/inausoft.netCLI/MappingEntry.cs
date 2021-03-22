using System;
using System.Collections.Generic;
using System.Linq;

namespace inausoft.netCLI
{
    public class CommandInfo
    {
        public CommandAttribute Command { get; internal set; }

        public IEnumerable<OptionAttribute> Options { get; internal set; }
    }

    internal class MappingEntry
    {
        public Type CommandType { get; set; }

        public Type HandlerType { get; set; }

        public object HandlerInstance { get; set; }
    }
}
