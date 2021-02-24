using System;
using System.Collections.Generic;
using System.Text;

namespace inausoft.netCLI.MateuszCommands
{
    public interface IMateuszCommandHandler<out T> where T : IMateuszCommand
    {
        T GetCommand();
        int Run();
    }
}
