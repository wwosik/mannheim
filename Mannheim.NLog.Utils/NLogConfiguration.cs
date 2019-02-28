using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Mannheim.NLog.Utils
{
    public static class NLogConfiguration
    {
        /// <summary>
        /// Opinionated configuration of NLog
        /// </summary>
        /// <param name="appFolder"></param>
        public static void ConfigureNLog(DirectoryInfo appFolder)
        {
            Console.Error.WriteLine("Configuring NLog with " + appFolder.FullName);
            var logFolder = new DirectoryInfo(Path.Combine(appFolder.FullName, "logs"));
            if (!logFolder.Exists)
            {
                logFolder.Create();
            }

            var config = new LoggingConfiguration();

            var microsoftFileTarget = new FileTarget("microsoft")
            {
                FileName = $@"{logFolder.FullName}/microsoft.${{shortdate}}.log",
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(microsoftFileTarget);
            config.AddRuleForAllLevels(microsoftFileTarget, "Microsoft.*", true);

            var consoleTarget = new ColoredConsoleTarget("console")
            {
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(consoleTarget);
            config.AddRuleForAllLevels(consoleTarget);

            var fileTarget = new FileTarget("file")
            {
                FileName = $@"{logFolder.FullName}/general.${{shortdate}}.log",
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(fileTarget);
            config.AddRuleForAllLevels(fileTarget);

            var errorFileTarget = new FileTarget("errorFileTarget")
            {
                FileName = $@"{logFolder.FullName}/error.${{shortdate}}.log",
                Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
            };
            config.AddTarget(errorFileTarget);
            config.AddRuleForOneLevel(global::NLog.LogLevel.Fatal, errorFileTarget);
            config.AddRuleForOneLevel(global::NLog.LogLevel.Error, errorFileTarget);
            config.AddRuleForOneLevel(global::NLog.LogLevel.Warn, errorFileTarget);

            if (System.Diagnostics.Debugger.IsAttached)
            {
                var debuggerTarget = new DebuggerTarget("debugger")
                {
                    Layout = @"${date:format=HH\:mm\:ss} ${level} ${message} ${exception}"
                };
                config.AddTarget(debuggerTarget);
                config.AddRuleForAllLevels(debuggerTarget);
            }

            LogManager.Configuration = config;
        }
    }
}
