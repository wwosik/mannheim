using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;

namespace Mannheim.Google.BatchRequests
{
    public class BatchRequestBuilder
    {
        public List<Request> Requests { get; }

        private readonly SpreadsheetsResource.BatchUpdateRequest request;
        private readonly SheetsService service;
        private readonly string spreadsheetId;

        public BatchRequestBuilder(SheetsService service, string spreadsheetId)
        {
            this.Requests = new List<Request>();
            this.service = service;
            this.spreadsheetId = spreadsheetId;
        }

        public SpreadsheetsResource.BatchUpdateRequest Build()
        {
            return this.service.Spreadsheets.BatchUpdate(new BatchUpdateSpreadsheetRequest
            {
                Requests = this.Requests
            }, this.spreadsheetId);
        }

        public Task BuildAndRun()
        {
            return this.Build().ExecuteAsync();
        }

        public DeleteRangeRequest AddDeleteRange(int sheetId, string shiftDimension = "ROWS")
        {
            var deleteRequest = new DeleteRangeRequest
            {
                Range = new GridRange
                {
                    SheetId = sheetId,
                },
                ShiftDimension = shiftDimension
            };
            this.Requests.Add(new Request { DeleteRange = deleteRequest });

            return deleteRequest;
        }

        public UpdateCellsRequest AddUpdateCells(int sheetId, string fields = "*")
        {
            var updateCellsRequest = new UpdateCellsRequest { Range = new GridRange { SheetId = sheetId }, Fields = fields, Rows = new List<RowData>() };
            this.Requests.Add(new Request { UpdateCells = updateCellsRequest });
            return updateCellsRequest;
        }

        public AppendCellsRequest AddAppendCells(int sheetId, string fields = "*")
        {
            var appendCellsRequest = new AppendCellsRequest { SheetId = sheetId, Fields = fields, Rows = new List<RowData>() };
            this.Requests.Add(new Request { AppendCells = appendCellsRequest });
            return appendCellsRequest;
        }

        public AutoResizeDimensionsRequest AddAutoResizeDimensions(int sheetId, string direction = "COLUMNS", int? first = null, int? last = null)
        {
            var request = new AutoResizeDimensionsRequest
            {
                Dimensions = new DimensionRange
                {
                    SheetId = sheetId,
                    StartIndex = first,
                    EndIndex = last,
                    Dimension = direction
                }
            };
            this.Requests.Add(new Request { AutoResizeDimensions = request });
            return request;
        }
    }
}
