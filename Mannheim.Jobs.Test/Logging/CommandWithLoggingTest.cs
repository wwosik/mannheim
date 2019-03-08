using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Jobs.Logging
{
    public class CommandWithLoggingTest
    {
        private readonly ITestOutputHelper testOutput;

        public CommandWithLoggingTest(ITestOutputHelper testOutput)
        {
            this.testOutput = testOutput;
        }

        [Fact]
        public void RunsAndLogs()
        {
            var testRunner = new TestRunner<CommandWithLogging>(this.testOutput, "CommandWithLoggingTest");
            testRunner.Run();
            var logLines = testRunner.ReadGeneralLog();
            Assert.Contains(logLines, l => l.Contains("INFORMATION MESSAGE"));
        }
    }
}
