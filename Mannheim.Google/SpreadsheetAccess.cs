using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Logging;
using Mannheim.Google.BatchRequests;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace Mannheim.Google
{
    public class SpreadsheetAccess
    {
        private readonly SheetsService service;
        private readonly SpreadsheetsResource.ValuesResource valuesResource;
        private readonly ILogger logger;

        public SpreadsheetAccess(SheetsService service, string spreadsheetId, ILogger logger)
        {
            this.service = service;
            this.valuesResource = service.Spreadsheets.Values;
            this.SpreadsheetId = spreadsheetId;
            this.logger = logger;
        }

        public string SpreadsheetId { get; }

        public ClearRequest CreateClearRangeRequest(string range)
        {
            return this.valuesResource.Clear(null, this.SpreadsheetId, range);
        }

        public async Task ClearRangeAsync(string range)
        {
            this.logger.LogTrace($"Clearing {range}...");
            var result = await this.CreateClearRangeRequest(range).ExecuteAsync();
            this.logger.LogTrace($"When clearing range {range}, cleared: {result.ClearedRange}");
        }

        public async Task AppendRangeAsync(string range, IList<IList<object>> data,
            AppendRequest.ValueInputOptionEnum valueInputOption = AppendRequest.ValueInputOptionEnum.RAW,
            AppendRequest.InsertDataOptionEnum insertInputOption = AppendRequest.InsertDataOptionEnum.INSERTROWS
            )
        {
            this.logger.LogTrace($"Putting values into range {range}...");
            var request = this.CreateAppendRangeRequest(range, data, valueInputOption, insertInputOption);
            var result = await request.ExecuteAsync();
            this.logger.LogTrace($"When appending range {range}, actual range: {result.Updates.UpdatedRange}");
        }

        public AppendRequest CreateAppendRangeRequest(string range, IList<IList<object>> data,
           AppendRequest.ValueInputOptionEnum valueInputOption = AppendRequest.ValueInputOptionEnum.RAW,
           AppendRequest.InsertDataOptionEnum insertInputOption = AppendRequest.InsertDataOptionEnum.INSERTROWS
           )
        {

            var appendResource = this.valuesResource.Append(new ValueRange { Values = data }, this.SpreadsheetId, range);
            appendResource.ValueInputOption = valueInputOption;
            appendResource.InsertDataOption = insertInputOption;
            return appendResource;
        }

        public BatchRequestBuilder CreateBatchRequestBuilder()
        {
            return new BatchRequestBuilder(this.service, this.SpreadsheetId);

        }

        public Task fsdf(int sheetId, IList<IList<object>> data)
        {
            var request = this.service.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest
            {
                Requests = new Request[] {
                    new Request
                    {
                        DeleteRange = new DeleteRangeRequest{
                            ShiftDimension= "ROWS",
                            Range = new GridRange{ StartColumnIndex = 1, EndColumnIndex = 1, SheetId = sheetId}
                            }
                    },
                    new Request {
                        AppendCells = new AppendCellsRequest    {
                            SheetId = sheetId,
                            Fields="*",
                            Rows =data.Select(d=>new RowData{
                                Values= d.Select(cellValue=>new CellData{
                                    UserEnteredValue = new ExtendedValue{StringValue= cellValue.ToString() } }).ToList()
                                     }).ToList()
                            }
    }
}
            }, this.SpreadsheetId);

            return request.ExecuteAsync();
        }
    }
}
