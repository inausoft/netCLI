using System;

namespace inausoft.netCLI
{
    public abstract class CommandHandler
    {
        internal abstract Type GetOptionsType();

        internal abstract int Run(object options);
    }

    public abstract class CommandHandler<T> : CommandHandler where T : class
    {
        public abstract int Run(T options);

        internal override int Run(object options)
        {
            return Run(options as T);
        }

        internal override Type GetOptionsType()
        {
            return typeof(T);
        }
    }
}
