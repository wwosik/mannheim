using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Jobs.Logging
{
    public class CommandWithLogging : ICommand
    {
        private readonly ILogger<CommandWithLogging> logger;

        public CommandWithLogging(ILogger<CommandWithLogging> logger)
        {
            this.logger = logger;
        }

        public Task Run()
        {
            this.logger.LogInformation("INFORMATION MESSAGE");
            this.logger.LogTrace("TRACE MESSAGE");
            return Task.CompletedTask;
        }
    }
}
