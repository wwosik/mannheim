using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Conditions;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mannheim.NLog.Utils
{
    public static class NLogConfiguration
    {
        /// <summary>
        /// Opinionated configuration of NLog
        /// </summary>
        /// <param name="appFolder"></param>
        public static void ConfigureNLog(DirectoryInfo logFolder, string filePrefix = "", FileNameFormat fileNameFormat = FileNameFormat.ShortDate)
        {
            if (!string.IsNullOrEmpty(filePrefix) && !filePrefix.EndsWith("-")) filePrefix += "-";

            Console.Error.WriteLine($"Configuring NLog with folder {logFolder.FullName} and prefix {filePrefix}");
            if (!logFolder.Exists)
            {
                logFolder.Create();
            }

            var config = new LoggingConfiguration();
            string filenameSuffix;
            switch (fileNameFormat)
            {
                case FileNameFormat.Simple:
                    filenameSuffix = "";
                    break;
                case FileNameFormat.ShortDate:
                default:
                    filenameSuffix = ".${shortdate}";
                    break;
            }

            var microsoftFileTarget = new FileTarget("microsoft")
            {
                FileName = $@"{logFolder.FullName}/{filePrefix}microsoft{filenameSuffix}.log",
                Layout = @"${date:format=HH\:mm\:ss} ${level:uppercase=true} ${logger} ${message} ${exception}"
            };
            config.AddTarget(microsoftFileTarget);
            config.AddRuleForAllLevels(microsoftFileTarget, "Microsoft.*", true);
            config.AddRuleForAllLevels(microsoftFileTarget, "System.Net.Http.HttpClient.*", true);

            var consoleTarget = new ColoredConsoleTarget("console")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level:uppercase=true} ${logger:shortName=true} ${message} ${exception}"
            };

            consoleTarget.RowHighlightingRules.Add(
                new ConsoleRowHighlightingRule
                {
                    Condition = ConditionParser.ParseExpression("level == LogLevel.Debug"),
                    ForegroundColor = ConsoleOutputColor.DarkCyan
                });
            consoleTarget.RowHighlightingRules.Add(
              new ConsoleRowHighlightingRule
              {
                  Condition = ConditionParser.ParseExpression("level == LogLevel.Trace"),
                  ForegroundColor = ConsoleOutputColor.DarkCyan
              });

            config.AddTarget(consoleTarget);
            config.AddRuleForAllLevels(consoleTarget);

            var fileTarget = new FileTarget("file")
            {
                FileName = $@"{logFolder.FullName}/{filePrefix}general{filenameSuffix}.log",
                Layout = @"${date:format=HH\:mm\:ss} ${level:uppercase=true} ${logger:shortName=true} ${message} ${exception}"
            };
            config.AddTarget(fileTarget);
            config.AddRuleForAllLevels(fileTarget);

            var errorFileTarget = new FileTarget("errorFileTarget")
            {
                FileName = $@"{logFolder.FullName}/error{filenameSuffix}.log",
                Layout = @"${date:format=HH\:mm\:ss} ${level:uppercase=true} ${logger:shortName=true} ${message} ${exception}"
            };
            config.AddTarget(errorFileTarget);
            config.AddRuleForOneLevel(global::NLog.LogLevel.Fatal, errorFileTarget);
            config.AddRuleForOneLevel(global::NLog.LogLevel.Error, errorFileTarget);
            config.AddRuleForOneLevel(global::NLog.LogLevel.Warn, errorFileTarget);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                var debuggerTarget = new DebuggerTarget("debugger")
                {
                    Layout = @"${date:format=HH\:mm\:ss} ${level:uppercase=true} ${logger:shortName=true} ${message} ${exception}"
                };
                config.AddTarget(debuggerTarget);
                config.AddRuleForAllLevels(debuggerTarget);
            }

            LogManager.Configuration = config;
        }

        public static void AddLoggingWithNLog(this IServiceCollection services)
        {
            services.AddLogging(l =>
               {
                   l.ClearProviders();
                   l.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   l.AddNLog();
               });
        }

    }

    public enum FileNameFormat
    {
        Simple,
        ShortDate,
    }
}
