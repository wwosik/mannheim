using Mannheim.NLog.Utils;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NLog;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Mannheim.Jobs
{
    public class Program
    {
        private static Logger logger = null;
        private static readonly string currentDirectory = Directory.GetCurrentDirectory();
        private static readonly JobStatusInfo jobStatusInfo = new JobStatusInfo
        {
            StartedAt = DateTime.Now,
            Status = "INITIALIZING"
        };
        private static readonly string jobStatusInfoPath
            = Path.Combine(currentDirectory, "job-status.json");

        private static JobStartInfo jobStartInfo;

        static Program()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                if (e.ExceptionObject is Exception ex)
                {
                    Console.Error.WriteLine(ex.Message);
                    Console.Error.WriteLine(ex.StackTrace);
                }
                else
                {
                    Console.Error.WriteLine("GENERAL ERROR");
                }
            };
        }

        public static int Main(string[] args)
        {
            try
            {
                SaveJobStatusInfo();

                logger = SetupNLog();
                if (logger == null) return Exit(ExitCodes.FailedToInitializeLogging);

                jobStartInfo = GetJobInfo(currentDirectory);
                if (jobStartInfo == null) return Exit(ExitCodes.FailedToLoadJobStartInfo);

                var (exitCode, commandType) = FindCommand(jobStartInfo);
                if (exitCode.HasValue) return exitCode.Value;

                if (!commandType.GetInterfaces().Any(i => i == typeof(ICommand)))
                {
                    logger.Fatal($"Indicated command type {jobStartInfo.CommandName} does not implement ICommand interface");
                    return Exit(ExitCodes.NotACommand);
                }

                ICommandServicesConfigurator commandServicesConfigurator = null;

                var commandAttribute = commandType.GetCustomAttribute<CommandAttribute>();
                if (commandAttribute != null)
                {
                    if (!string.IsNullOrEmpty(commandAttribute.JobStartInfoType))
                    {
                        var jobStartInfoType = commandType.Assembly.GetType(commandAttribute.JobStartInfoType);
                        if (jobStartInfoType == null)
                        {
                            logger.Fatal($"Did not find JobStartInfoType {commandAttribute.JobStartInfoType} indicated by command {commandType.FullName}");
                            return Exit(ExitCodes.FailedToLoadJobStartInfo);
                        }

                        jobStartInfo = GetJobInfo(currentDirectory, jobStartInfoType);
                        if (jobStartInfo == null)
                        {
                            return Exit(ExitCodes.FailedToLoadJobStartInfo);
                        }
                    }

                    if (!string.IsNullOrEmpty(commandAttribute.CommandServicesConfigurator))
                    {
                        var commandServicesConfiguratorType = commandType.Assembly.GetType(commandAttribute.CommandServicesConfigurator);
                        if (commandServicesConfiguratorType == null)
                        {
                            logger.Fatal($"Did not find ICommandServicesConfigurator {commandAttribute.CommandServicesConfigurator} indicated by command {commandType.FullName}");
                            return Exit(ExitCodes.FailedToInitializeServices);
                        }

                        try
                        {
                            commandServicesConfigurator = (ICommandServicesConfigurator)Activator.CreateInstance(commandServicesConfiguratorType);
                        }
                        catch (Exception ex)
                        {
                            logger.Fatal($"Failed to initialize ICommandServiceConfigurator {commandServicesConfiguratorType}: {ex.GetBaseException().Message}");
                            return Exit(ExitCodes.FailedToInitializeServices);
                        }
                    }
                }

                IServiceProvider serviceProvider;
                try
                {
                    var services = new ServiceCollection();

                    services.AddLogging(l =>
                    {
                        l.ClearProviders();
                        l.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                        l.AddNLog();
                    });
                    services.AddSingleton<JobStartInfo>(jobStartInfo);

                    commandServicesConfigurator?.Initialize(services);

                    serviceProvider = services.BuildServiceProvider();
                }
                catch (Exception ex)
                {
                    logger.Fatal($"Failed to initialize services: {ex.GetBaseException().Message}");
                    return Exit(ExitCodes.FailedToInitializeServices);
                }

                ICommand command;
                try
                {
                    command = (ICommand)ActivatorUtilities.CreateInstance(serviceProvider, commandType);
                }
                catch (Exception ex)
                {
                    logger.Fatal($"Failed to initialize services: {ex.GetBaseException().Message}");
                    return Exit(ExitCodes.FailedToInitializeCommand);
                }

                try
                {
                    logger.Info("=== STARTING COMMAND EXECUTION");
                    command.Run().Wait();
                    return Exit(ExitCodes.Success);
                }
                catch (Exception ex)
                {
                    logger.Error("=== FAILED IN EXECUTION");
                    if (ex is AggregateException) { ex = ((AggregateException)ex).InnerException; }
                    while (ex != null)
                    {
                        logger.Error(ex.Message);
                        logger.Error(ex.StackTrace);
                        ex = ex.InnerException;
                    }
                    return Exit(ExitCodes.FailureInExecution);
                }
            }
            catch (Exception ex)
            {
                logger?.Fatal(ex, ex.Message);
                return Exit(ExitCodes.GeneralError);
            }
            finally
            {
                try
                {
                    LogManager.Shutdown();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to shutdown logging: " + ex.GetBaseException().Message);
                }

                Console.Error.WriteLine("===== DONE");
                if (Debugger.IsAttached && !(jobStartInfo?.DisableDebugging == true))
                {
                    Console.ReadLine();
                }
            }



        }

        private static int Exit(ExitCodes exitCode)
        {
            try
            {
                jobStatusInfo.CompletedAt = DateTime.Now;
                if (exitCode != ExitCodes.Success)
                {
                    jobStatusInfo.Status = "FAILED: " + exitCode.ToString();
                    logger.Fatal("==== COMPLETED AS FAILED: " + exitCode.ToString());
                }
                else
                {
                    jobStatusInfo.Status = "COMPLETED";
                    logger.Info("===== COMPLETED");
                }

                SaveJobStatusInfo();

                return (int)exitCode;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to save status: " + ex.GetBaseException().Message);
            }

            return (int)exitCode;
        }

        private static void SaveJobStatusInfo()
        {
            jobStatusInfo.LastUpdate = DateTime.Now;
            File.WriteAllText(jobStatusInfoPath, JsonConvert.SerializeObject(jobStatusInfo, Formatting.Indented));
        }

        private static JobStartInfo GetJobInfo(string currentDirectory, Type type = null)
        {
            var path = Path.Combine(currentDirectory, "job-start-info.json");
            try
            {
                if (type == null)
                {
                    type = typeof(JobStartInfo);
                }

                return (JobStartInfo)JsonConvert.DeserializeObject(File.ReadAllText(path), type);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex, $"Could not load {type.FullName} from {path}: {ex.GetBaseException().Message}");
                return null;
            }
        }

        private static Logger SetupNLog()
        {
            NLogConfiguration.ConfigureNLog(new DirectoryInfo(currentDirectory), fileNameFormat: FileNameFormat.Simple);
            var logger = LogManager.GetLogger("Mannheim.Jobs");
            logger.Debug("Logging established");
            return logger;
        }

        private static (int?, Type) FindCommand(JobStartInfo jobStartInfo)
        {
            foreach (var assemblyFilePath in jobStartInfo.Assemblies ?? Enumerable.Empty<string>())
            {
                logger.Info("Loading assembly " + assemblyFilePath);
                try
                {
                    var assembly = Assembly.LoadFrom(assemblyFilePath);
                    var commandType = assembly.GetTypes().FirstOrDefault(t => t.FullName == jobStartInfo.CommandName);
                    if (commandType != null)
                    {
                        logger.Info($"Found command {jobStartInfo.CommandName} in assembly {assembly.FullName}");
                        return (null, commandType);
                    }
                }
                catch (Exception ex)
                {
                    logger.Fatal(ex, "Failed to load assembly " + assemblyFilePath);
                    return ((int)ExitCodes.FailedToLoadAssembly, null);
                }
            }

            logger.Fatal($"Did not find command {jobStartInfo.CommandName} in any of assemblies: {string.Join(", ", jobStartInfo.Assemblies)}");
            return ((int)ExitCodes.FailedToFindCommand, null);
        }
    }

    public enum ExitCodes
    {
        Success = 0,
        GeneralError = 128,
        FailureInExecution = 1,
        FailedToInitializeLogging = 2,
        FailedToLoadJobStartInfo = 3,
        FailedToLoadAssembly = 4,
        FailedToFindCommand = 5,
        NotACommand = 6,
        FailedToInitializeServices = 7,
        FailedToInitializeCommand = 8,
    }
}
