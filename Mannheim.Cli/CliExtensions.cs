using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Mannheim.Cli
{
    public static class CliExtensions
    {
        public static void AddCli(this IServiceCollection services, string[] args, Assembly commandsAssembly)
        {
            services.AddSingleton(new OriginalArguments(args));
            services.AddSingleton<ICommandOutput, CommandOutput>();
            services.AddSingleton<ICommandInput, CommandInput>();
            services.AddSingleton<ICommandFactory, CommandFactory>(sp => new CommandFactory(commandsAssembly));
        }
    }
}
