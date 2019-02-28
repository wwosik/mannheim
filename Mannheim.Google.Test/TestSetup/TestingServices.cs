using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Mannheim.Google.TestSetup
{
    public static class TestingServices
    {
        private static readonly ServiceProvider servicesProvider;
        //private static SalesforceClient cachedClient;

        public static GoogleTestOptions Options { get; }


        private static readonly object lockObject = new object();
        private static LoggerOutput loggerOutput;

        internal static void SetLogger(ITestOutputHelper output)
        {
            if (loggerOutput != null)
            {
                loggerOutput.SetOutput(output);
                return;
            }

            lock (lockObject)
            {
                if (loggerOutput == null)
                {
                    loggerOutput = new LoggerOutput(output);
                    global::Google.ApplicationContext.RegisterLogger(new Logger(loggerOutput));
                }
                else
                {
                    loggerOutput.SetOutput(output);
                }
            }
        }

        static TestingServices()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<GoogleTestOptions>(false);

            var config = builder.Build();
            Options = config.Get<GoogleTestOptions>();

            var services = new ServiceCollection();
            services.AddHttpClient();

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

        public static async Task<UserCredential> AuthorizeAsync()
        {
            var folder = new DirectoryInfo(Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Mannheim.Google.Test"));

            if (!folder.Exists)
            {
                folder.Create();
            }

            var source = new CancellationTokenSource(30 * 1000);

            var credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                Options.GoogleOAuth.ToGoogleApi(), new[] {
                    SheetsService.Scope.Spreadsheets,
                    global::Google.Apis.Drive.v3.DriveService.Scope.DriveMetadataReadonly
                    },
                "user",
                source.Token, new global::Google.Apis.Util.Store.FileDataStore(folder.FullName)
                );

            return credential;
        }
    }
}
