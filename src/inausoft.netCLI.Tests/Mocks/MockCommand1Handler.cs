using System;
using System.Collections.Generic;
using System.Text;

namespace inausoft.netCLI.Tests.Mocks
{
    class MockCommand1Handler : CommandHandler<Command1>
    {
        public Command1 LastRunParameters { get; private set; }

        public override int Run(Command1 command)
        {
            LastRunParameters = command;

            return 0;
        }
    }
}
