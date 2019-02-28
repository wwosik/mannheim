using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mannheim.DataProtection
{
    public class TestingServices
    {
        private static readonly ServiceProvider servicesProvider;

        static TestingServices()
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDataProtection();

            services.Configure<DataProtectedFileStoreOptions>(o =>
            {
                o.Folder = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mannheim.DataProtectedFileStore.Test"));
                o.ProtectorKey = "Mannheim.DataProtectedFileStore.Test";
            });

            services.AddSingleton<DataProtectedFileStore>();

            servicesProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>()
        {
            return servicesProvider.GetRequiredService<T>();
        }

        public static T Build<T>()
        {
            return ActivatorUtilities.CreateInstance<T>(servicesProvider);
        }
    }
}
