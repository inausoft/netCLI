using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace netCLISample
{
    public sealed class MyConsoleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new CustomConsoleLogger();
        }

        public void Dispose()
        {
            
        }
    }


    class CustomConsoleLogger : ILogger
    {
        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            Console.WriteLine($"{formatter(state, exception)}");
        }
    }
}
