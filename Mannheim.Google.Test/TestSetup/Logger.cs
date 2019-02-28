using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Logging;
using Xunit.Abstractions;

namespace Mannheim.Google.TestSetup
{
    public class Logger : global::Google.Apis.Logging.BaseLogger
    {
        private readonly LoggerOutput output;

        public Logger(LoggerOutput output, Type t = null) : base(LogLevel.Debug, global::Google.Apis.Util.SystemClock.Default, t)
        {
            this.output = output;
        }

        protected override ILogger BuildNewLogger(Type type)
        {
            return new Logger(this.output, type);
        }

        protected override void Log(LogLevel logLevel, string formattedMessage)
        {
            this.output.Output.WriteLine($"{logLevel.ToString().ToUpperInvariant()} {formattedMessage}");
        }
    }

    public class LoggerOutput
    {
        public LoggerOutput(ITestOutputHelper output)
        {
            this.Output = output;
        }

        public ITestOutputHelper Output { get; private set; }

        public void SetOutput(ITestOutputHelper newOutput)
        {
            this.Output = newOutput;
        }
    }
}
