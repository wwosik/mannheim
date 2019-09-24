using Mannheim.NLog.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mannheim.Cli
{
    public class CliProgram
    {
        private readonly IServiceCollection services;
        private readonly ServiceProvider sp;
        private readonly ILogger<CliProgram> logger;

        public CliProgram(IServiceCollection services)
        {
            this.services = services;
            this.sp = services.BuildServiceProvider();
            this.logger = sp.GetRequiredService<ILogger<CliProgram>>();

        }

        public void Run()
        {
            var command = PrepareCommand();
            RunCommand(command);
        }


        private ICommand PrepareCommand()
        {
            try
            {
                var arguments = sp.GetRequiredService<OriginalArguments>();
                var commandFactory = sp.GetRequiredService<ICommandFactory>();
                var configuration = sp.GetRequiredService<IConfigurationRoot>();

                var commandName = arguments[0];
                if (string.IsNullOrEmpty(commandName))
                {
                    var output = sp.GetRequiredService<ICommandOutput>();
                    output.WriteLine(commandFactory.AvailableCommandsText);
                    return null;
                }

                var commandInfo = commandFactory.FindCommand(commandName);

                if (commandInfo == null)
                {
                    logger.LogError($"Command {arguments[0]} not found. Available commands: {commandFactory.AvailableCommandsText}");
                    return null;
                }

                commandInfo.Configure(services, configuration);
                return commandInfo.CreateInstance(services.BuildServiceProvider());
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed trying to initialize command");
                throw;
            }
        }

        private void RunCommand(ICommand command)
        {
            if (command == null) return;

            try
            {
                command.RunAsync().Wait();
            }
            catch (Exception ex)
            {
                while (ex is AggregateException) ex = ((AggregateException)ex).InnerException;

                logger.LogError($"===== Command ended in error: {ex.GetBaseException().Message}");
                logger.LogError(ex.StackTrace);

                throw;
            }
        }
    }
}
