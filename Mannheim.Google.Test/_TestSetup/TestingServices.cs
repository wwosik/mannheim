using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Mannheim.XUnit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace Mannheim.Google
{
    public class TestingServices : TestServicesBase
    {
        public static GoogleTestOptions Options { get; }

        static TestingServices()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<GoogleTestOptions>(false);

            var config = builder.Build();
            Options = config.Get<GoogleTestOptions>();
        }

        public TestingServices(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        public async Task<UserCredential> AuthorizeAsync()
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

        public override void ConfigureTestSpecificServices(IServiceCollection services)
        {
            var logger = services.BuildServiceProvider().GetRequiredService<ILogger<GoogleLogForwarder>>();
            global::Google.ApplicationContext.RegisterLogger(new GoogleLogForwarder(logger));

        }
    }
}
