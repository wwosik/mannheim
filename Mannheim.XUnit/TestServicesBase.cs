using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Mannheim.XUnit
{
    public abstract class TestServicesBase
    {
        public TestServicesBase(ITestOutputHelper testOutputHelper)
        {
            var services = new ServiceCollection();
            services.AddHttpClient();
            services.AddLogging(logging =>
            {
                logging.SetMinimumLevel(LogLevel.Trace);

                logging.ClearProviders();
                logging.AddProvider(new TestOutputLoggerProvider(testOutputHelper));
                this.OnConfigureLogging(logging);
            });

            this.ConfigureTestSpecificServices(services);
            this.ServicesProvider = services.BuildServiceProvider();
        }

        public ServiceProvider ServicesProvider { get; }

        public abstract void ConfigureTestSpecificServices(IServiceCollection services);

        public virtual void OnConfigureLogging(ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddFilter("Microsoft.AspNetCore.DataProtection",
                level => (level >= LogLevel.Information)
                );

            loggingBuilder.AddFilter("System.Net.Http.HttpClient.Default",
              LogLevel.Information
              );

        }

        public T GetRequiredService<T>()
        {
            return this.ServicesProvider.GetRequiredService<T>();
        }

        public T Build<T>()
        {
            return ActivatorUtilities.CreateInstance<T>(this.ServicesProvider);
        }
    }
}
