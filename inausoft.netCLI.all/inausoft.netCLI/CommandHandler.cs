using System;

namespace inausoft.netCLI
{
    public interface ICommandHandler
    {
        Type GetCommandType();

        int Run(object command);
    }

    public abstract class CommandHandler<T> : ICommandHandler where T : class
    {
        public abstract int Run(T options);

        public int Run(object command)
        {
            return Run(command as T);
        }

        public Type GetCommandType()
        {
            return typeof(T);
        }
    }
}
