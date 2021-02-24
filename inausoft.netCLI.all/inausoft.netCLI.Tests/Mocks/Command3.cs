using System;
using System.Collections.Generic;
using System.Text;

namespace inausoft.netCLI.Tests.Mocks
{
    [Command("command3")]

    class Command3
    {
        [Option("RequiredArgument")]
        public bool RequiredArgument { get; set; }

        [Option("OptionalArgument")]
        public bool OptionalArgument { get; set; }
    }
}
