using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Mannheim.XUnit
{
    public class TestOutputLoggerProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper testOutput;

        public TestOutputLoggerProvider(ITestOutputHelper testOutput)
        {
            this.testOutput = testOutput;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new TestOutputLogger(categoryName, this.testOutput);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
