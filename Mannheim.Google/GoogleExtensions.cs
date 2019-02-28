using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Apis.Sheets.v4.Data;

namespace Mannheim.Google
{
    public static class GoogleExtensions
    {
        public static int GetSheetId(this Spreadsheet spreadsheet, string title)
        {
            return GetSheet(spreadsheet, title).SheetId.Value;
        }

        public static SheetProperties GetSheet(this Spreadsheet spreadsheet, string title)
        {
            return spreadsheet.Sheets.First(s => s.Properties.Title == title).Properties;
        }
    }
}
