using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using global::Google.Apis.Services;
using global::Google.Apis.Sheets.v4;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Google
{
    public class TryConnection
    {
        private readonly TestingServices services;

        public TryConnection(ITestOutputHelper output)
        {
            this.services = new TestingServices(output);
        }

        [Fact]
        public async Task FindASheet()
        {
            var credential = await this.services.AuthorizeAsync();

            var drive = new global::Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Mannheim.Google"
            });

            var logger = this.services.GetRequiredService<ILogger<TryConnection>>();
            var request = await drive.Files.List().ExecuteAsync();
            var file = request.Files.
                First(f => f.MimeType == "application/vnd.google-apps.spreadsheet");

            logger.LogInformation($"{file.Id} {file.Name}");

            var sheetService = new global::Google.Apis.Sheets.v4.SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Mannheim.Google"
            });

            var spreadsheetFile = await sheetService.Spreadsheets.Get(file.Id).ExecuteAsync();
            logger.LogInformation(JsonConvert.SerializeObject(spreadsheetFile));
        }
    }
}
