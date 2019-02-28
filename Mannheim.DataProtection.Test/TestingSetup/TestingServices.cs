using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Mannheim.XUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Mannheim.DataProtection
{
    public class TestingServices : TestServicesBase
    {
        public TestingServices(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        public override void ConfigureTestSpecificServices(IServiceCollection services)
        {
            services.AddDataProtection();

            services.Configure<DataProtectedFileStoreOptions>(o =>
            {
                o.Folder = new DirectoryInfo(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Mannheim.DataProtectedFileStore.Test"));
                o.ProtectorKey = "Mannheim.DataProtectedFileStore.Test";
            });

            services.AddDataProtectedFileStore();
        }
    }
}
