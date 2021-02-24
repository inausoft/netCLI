using System;
using System.Collections.Generic;
using System.Text;

namespace inausoft.netCLI.Tests.Mocks
{
    class MockCommand3Handler : CommandHandler<Command3>
    {
        public Command3 LastRunParameters { get; private set; }

        public override int Run(Command3 command)
        {
            LastRunParameters = command;

            return 0;
        }
    }
}
