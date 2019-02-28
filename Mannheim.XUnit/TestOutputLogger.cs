using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Mannheim.XUnit
{
    public class TestOutputLogger : ILogger
    {
        private readonly string categoryName;
        private readonly ITestOutputHelper testOutput;

        public TestOutputLogger(string categoryName, ITestOutputHelper testOutput)
        {
            this.categoryName = categoryName;
            this.testOutput = testOutput;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            throw new NotSupportedException();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!this.IsEnabled(logLevel)) return;

            var message = formatter(state, exception);
            this.testOutput.WriteLine($"{eventId.Id} {eventId.Name} {logLevel.ToString().ToUpperInvariant()} {this.categoryName} {message}");
        }
    }
}