using Mannheim.NLog.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Mannheim.Cli
{
    public class CliProgramBuilder
    {
        public DirectoryInfo RootDirectory { get; }

        private readonly Logger logger;
        private readonly List<Action<ConfigurationBuilder>> configurators = new List<Action<ConfigurationBuilder>>();
        private readonly List<Action<IServiceCollection>> servicesConfigurators = new List<Action<IServiceCollection>>();
        private readonly List<Assembly> commandAssemblies = new List<Assembly>();
        private OriginalArguments originalArguments;

        public CliProgramBuilder(string rootSubdirectoryName) : this(CreateLocalApplicationDataSubdirectory(rootSubdirectoryName))
        {
        }

        public CliProgramBuilder(DirectoryInfo rootDirectory)
        {
            if (rootDirectory is null) throw new ArgumentNullException(nameof(rootDirectory));

            this.RootDirectory = rootDirectory;
            this.logger = PrepareLogging(rootDirectory);
        }

        public void AddCommandLineArguments(string[] args)
        {
            this.originalArguments = new OriginalArguments(args);
        }

        public void Configure(Action<ConfigurationBuilder> action)
        {
            configurators.Add(action);
        }

        public void ConfigureServices(Action<IServiceCollection> servicesConfigurator)
        {
            servicesConfigurators.Add(servicesConfigurator);
        }

        public void AddCommandAssembly(Assembly commandAssembly)
        {
            this.commandAssemblies.Add(commandAssembly);
        }

        public CliProgram Build()
        {
            try
            {
                var configurationBuilder = new ConfigurationBuilder();

                foreach (var configurator in this.configurators)
                {
                    configurator(configurationBuilder);
                }

                var configuration = configurationBuilder.Build();

                var services = new ServiceCollection();
                services.AddSingleton(configuration);
                services.AddLoggingWithNLog();
                services.AddOptions();
                services.AddSingleton<ICommandOutput, CommandOutput>();
                services.AddSingleton<ICommandInput, CommandInput>();
                services.AddSingleton(this.originalArguments ?? new OriginalArguments(Array.Empty<string>()));

                foreach (var servicesConfigurator in this.servicesConfigurators)
                {
                    servicesConfigurator(services);
                }

                var commandFactory = new CommandFactory(this.commandAssemblies);
                services.AddSingleton<ICommandFactory>(commandFactory);


                return new CliProgram(services);
            }
            catch (Exception ex)
            {
                this.logger.Error($"Failed to construct program: {ex.GetBaseException().Message}");
                throw;
            }
        }



        private static DirectoryInfo CreateLocalApplicationDataSubdirectory(string rootSubdirectoryName)
        {
            var appData = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
            try
            {
                return appData.CreateSubdirectory(rootSubdirectoryName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot create root directory {rootSubdirectoryName} at {appData.FullName}");
                Console.WriteLine(ex.GetBaseException().Message);
                throw;
            }
        }

        private static Logger PrepareLogging(DirectoryInfo rootDirectory)
        {
            try
            {

                NLogConfiguration.ConfigureNLog(rootDirectory.CreateSubdirectory("log"));
                var coreLogger = LogManager.GetLogger("CORE");
                coreLogger.Trace("Root directory: " + rootDirectory.FullName);
                coreLogger.Info($"Called: " + Environment.CommandLine);
                return coreLogger;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to initialize logging: " + ex.Message);
                throw;
            }
        }
    }
}
