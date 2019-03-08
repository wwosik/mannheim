using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Jobs.Simple
{
    public class SimpleCommandTest
    {
        private readonly ITestOutputHelper testOutput;

        public SimpleCommandTest(ITestOutputHelper testOutput)
        {
            this.testOutput = testOutput;
        }

        [Fact]
        public void Run()
        {
            var testRunner = new TestRunner<SimpleCommand>(this.testOutput, "Simple");
            testRunner.Run();
        }
    }
}
