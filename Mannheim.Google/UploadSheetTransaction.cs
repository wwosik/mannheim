using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Logging;
using Mannheim.Google.BatchRequests;

namespace Mannheim.Google
{
    public class UploadSheetTransaction
    {
        public static async Task<UploadSheetTransaction> CreateUploadSheetAsync(SheetsService service, string spreadsheetId, string sheetTitle, ILogger logger)
        {
            var spreadsheetAccess = new SpreadsheetAccess(service, spreadsheetId, logger);
            var spreadsheetResource = service.Spreadsheets.Get(spreadsheetId);
            var spreadsheetInfo = await spreadsheetResource.ExecuteAsync();

            var sheet = spreadsheetInfo.GetSheet(sheetTitle);
            return new UploadSheetTransaction(spreadsheetAccess, sheet);
        }

        private readonly BatchRequestBuilder requestBuilder;
        private readonly int sheetId;
        private readonly RowData firstRow;
        private readonly AppendCellsRequest appender;
        private readonly SpreadsheetAccess spreadsheetAccess;

        public UploadSheetTransaction(SpreadsheetAccess spreadsheetAccess, SheetProperties sheet)
        {
            this.requestBuilder = spreadsheetAccess.CreateBatchRequestBuilder();
            this.sheetId = sheet.SheetId.Value;
            var deleteRange = this.requestBuilder.AddDeleteRange(this.sheetId);
            deleteRange.Range.StartRowIndex = 1;
            deleteRange.Range.EndRowIndex = sheet.GridProperties.RowCount - 1;

            var cellsUpdater = this.requestBuilder.AddUpdateCells(this.sheetId);
            this.firstRow = cellsUpdater.Rows.AddRow();

            this.appender = this.requestBuilder.AddAppendCells(this.sheetId);

            this.requestBuilder.AddAutoResizeDimensions(this.sheetId);
            this.spreadsheetAccess = spreadsheetAccess;
        }

        public Action<CellFormat> FormatHeader = cf =>
        {
            cf.BackgroundColor = new Color { Blue = 0.5f };
            cf.TextFormat = new TextFormat { FontFamily = "Roboto", ForegroundColor = new Color { Blue = 1, Red = 1, Green = 1 } };
            cf.HorizontalAlignment = "Center";

        };

        public async Task LoadDataAsync<T>(IEnumerable<T> dbValues, Func<T, IEnumerable<object>> getter, int batchSize)
        {
            var batches = this.Batch(dbValues, batchSize).ToList();
            foreach (var dbValue in batches[0])
            {
                var row = this.appender.Rows.AddRow();
                foreach (var cellValue in getter(dbValue))
                {
                    row.AddCell().Format(this.FormatCell).SetValue(cellValue);
                }
            }

            await this.requestBuilder.BuildAndRun();

            foreach (var batch in batches.Skip(1))
            {
                var batchRequest = this.spreadsheetAccess.CreateBatchRequestBuilder();
                var batchAppender = batchRequest.AddAppendCells(this.sheetId);
                foreach (var dbValue in batch)
                {
                    var row = batchAppender.Rows.AddRow();
                    foreach (var cellValue in getter(dbValue))
                    {
                        row.AddCell().Format(this.FormatCell).SetValue(cellValue);
                    }
                }

                await batchRequest.BuildAndRun();
            }
        }

        public Action<CellFormat> FormatCell = cf =>
      {
          cf.BackgroundColor = new Color { Blue = 0.9f, Green = 0.9f, Red = 0.9f };
          cf.TextFormat = new TextFormat { FontFamily = "Roboto", ForegroundColor = new Color { Blue = 0.1f, Red = 0.1f, Green = 0.1f } };
      };


        public void SetFirstRow(params string[] titles)
        {
            foreach (var title in titles)
            {
                this.firstRow.AddCell().Format(this.FormatHeader).SetValue(title);
            }
        }

        public RowData AddRow(params Func<object>[] valueGetters)
        {
            var row = this.appender.Rows.AddRow();
            foreach (var getter in valueGetters)
            {
                var cell = row.AddCell().Format(this.FormatCell).SetValue(getter());
            }

            return row;
        }

        public Task ExecuteAsync()
        {
            return this.requestBuilder.BuildAndRun();
        }

        private IEnumerable<IList<T>> Batch<T>(IEnumerable<T> dbValues, int batchSize)
        {
            var output = new List<T>(batchSize);
            foreach (var item in dbValues)
            {
                if (output.Count >= batchSize)
                {
                    yield return output;
                    output = new List<T>(batchSize);
                }

                output.Add(item);
            }

            if (output.Count >= 0)
            {
                yield return output;
            }
        }
    }
}
