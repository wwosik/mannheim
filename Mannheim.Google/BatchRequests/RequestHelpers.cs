using System;
using System.Collections.Generic;
using System.Text;
using Google.Apis.Sheets.v4.Data;

namespace Mannheim.Google.BatchRequests
{
    public static class RequestHelpers
    {
        public static RowData AddRow(this IList<RowData> rows)
        {
            var rowData = new RowData { Values = new List<CellData>() };
            rows.Add(rowData);
            return rowData;
        }

        public static CellData AddCell(this RowData row, Action<CellData> action = null)
        {
            var cellData = new CellData { };
            action?.Invoke(cellData);
            row.Values.Add(cellData);
            return cellData;
        }

        public static CellData Format(this CellData cell, Action<CellFormat> action = null)
        {
            cell.UserEnteredFormat = new CellFormat();
            action(cell.UserEnteredFormat);
            return cell;
        }

        public static CellData SetValue(this CellData cell, object value)
        {
            if (value == null)
            {
                cell.SetValue("");
            }
            else if (value is int)
            {
                cell.SetValue((int)value);
            }
            else if (value is float)
            {
                cell.SetValue((float)value);
            }
            else if (value is Hyperlink)
            {
                cell.SetValue((Hyperlink)value);
            }
            else if (value is bool)
            {
                cell.UserEnteredValue = new ExtendedValue { BoolValue = (bool)value };
            }
            else
            {
                cell.SetValue(value.ToString());
            }
            return cell;
        }

        public static CellData SetValue(this CellData cell, string textValue)
        {
            cell.UserEnteredValue = new ExtendedValue { StringValue = textValue };
            return cell;
        }

        public static CellData SetValue(this CellData cell, Hyperlink hyperlink)
        {
            cell.UserEnteredValue = new ExtendedValue { FormulaValue = "=HYPERLINK(\"" + hyperlink.Url.AbsoluteUri + "\",\"" + hyperlink.Title + "\")" };
            return cell;
        }

        public static CellData SetValue(this CellData cell, double? numberValue)
        {
            cell.UserEnteredValue = new ExtendedValue { NumberValue = numberValue };
            return cell;
        }
    }
}
