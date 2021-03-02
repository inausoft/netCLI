namespace inausoft.netCLI
{
    public interface ICommandHandler
    {
        int Run(object command);
    }

    public abstract class CommandHandler<T> : ICommandHandler where T : class
    {
        public abstract int Run(T options);

        public int Run(object command)
        {
            return Run(command as T);
        }
    }
}
