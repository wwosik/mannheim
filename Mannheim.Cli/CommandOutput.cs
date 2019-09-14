using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mannheim.Cli
{
    public class CommandOutput : ICommandOutput
    {
        private readonly ILogger<CommandOutput> logger;

        public CommandOutput(ILogger<CommandOutput> logger)
        {
            this.logger = logger;
        }

        public void WriteLine(string message)
        {
            Console.WriteLine(message);
            this.logger.LogTrace(message);
        }
    }
}
