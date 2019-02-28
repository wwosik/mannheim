using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using global::Google.Apis.Services;
using global::Google.Apis.Sheets.v4;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;

namespace Mannheim.Google
{
    public class TryConnection
    {
        private readonly ITestOutputHelper output;

        public TryConnection(ITestOutputHelper output)
        {
            this.output = output;
            TestSetup.TestingServices.SetLogger(output);
        }

        [Fact]
        public async Task EnumerateSheets()
        {
            var credential = await TestSetup.TestingServices.AuthorizeAsync();

            var drive = new global::Google.Apis.Drive.v3.DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Mannheim.Google"
            });

            var request = await drive.Files.List().ExecuteAsync();
            string fileId = null;
            foreach (var file in request.Files)
            {
                if (file.MimeType == "application/vnd.google-apps.spreadsheet")
                {
                    this.output.WriteLine(file.Name);
                    fileId = file.Id;
                    break;
                }
            }

            if (fileId == null)
            {
                throw new Exception();
            }

            var sheetService = new global::Google.Apis.Sheets.v4.SheetsService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "Mannheim.Google"
            });

            var spreadsheetFile = await sheetService.Spreadsheets.Get(fileId).ExecuteAsync();
        }
    }
}
