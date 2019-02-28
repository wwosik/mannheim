using System;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.XUnit.Test
{
    public class WriteMessageTest
    {
        private readonly XunitTestServices TestServices;

        public WriteMessageTest(ITestOutputHelper output)
        {
            this.TestServices = new XunitTestServices(output);
        }

        [Fact]
        public void LogDebug()
        {
            var logger = this.TestServices.GetRequiredService<ILogger<WriteMessageTest>>();
            logger.LogDebug("Debug Message");

        }

        [Fact]
        public void LogTrace()
        {
            var logger = this.TestServices.GetRequiredService<ILogger<WriteMessageTest>>();
            logger.LogTrace("Trace message");

        }
    }
}
