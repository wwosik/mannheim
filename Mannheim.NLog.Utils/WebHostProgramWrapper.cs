using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

namespace Mannheim.NLog.Utils
{
    public static class WebHostProgramWrapper
    {
        public static void RunWithNLog(string path, Func<IWebHostBuilder> builderProvider)
        {
            NLogConfiguration.ConfigureNLog(new DirectoryInfo(Environment.GetEnvironmentVariable("MAIN_DIR") ?? "."));
            var logger = LogManager.GetLogger("Program");

            try
            {
                var builder = builderProvider();
                var webHost = builder
                    .ConfigureLogging(logging =>
                    {
                        logging.ClearProviders();
                        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                    })
                    .UseNLog()
                    .Build();

                webHost.Run();
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
                if (System.Diagnostics.Debugger.IsAttached)
                {
                    Console.WriteLine("PRESS ENTER");
                    Console.ReadLine();
                }
            }
        }
    }
}
