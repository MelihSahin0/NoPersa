using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenXml.Exceptions;
using System.Globalization;

namespace OpenXml
{
    public class SpreadSheetCell
    {
        private readonly SharedStringTablePart sharedStringTablePart;
        private readonly Stylesheet stylesheet;

        public SpreadSheetCell(SharedStringTablePart sharedStringTablePart, Stylesheet stylesheet)
        {
            this.sharedStringTablePart = sharedStringTablePart;
            this.stylesheet = stylesheet;
        }

        public Cell Create<T>(T data)
        {
            if (data is string)
            {
                return Create((string?)(object?)data);
            }
            if (data is int)
            {
                return Create((int?)(object?)data);
            }
            if (data is double)
            {
                return Create((double?)(object?)data);
            }
            if (data is DateTime)
            {
                return Create((DateTime)(object)data);
            }
            if (data is bool)
            {
                return Create((bool)(object)data);
            }

            throw new NotSupportedException();
        }

        public T? Read<T>(Cell cell)
        {
            CellValues? dataType = (cell.DataType?.Value) ?? throw new ExcelException("Cell data type not found");
           
            string? value = cell.CellValue?.Text;

            if (dataType == CellValues.SharedString) 
            {
                return (T?)(object?)ReadString(value);
            }
            if (dataType == CellValues.Date)
            {
                return (T?)(object?)ReadDate(value);
            }
            if (dataType == CellValues.Boolean)
            {
                return (T?)(object?)(value);
            }
            if (dataType == CellValues.Number)
            {
                if (CheckForDateFormat(cell))
                {
                    return (T?)(object?)ReadDate(value);
                }
                else
                {
                    if (value?.Contains('.') == true || value?.Contains(',') == true)
                    {
                        Console.WriteLine((T?)(object?)ReadDouble(value));
                        return (T?)(object?)ReadDouble(value);
                    }
                    else
                    {
                        return (T?)(object?)ReadInt(value);
                    }
                }
            }

            throw new ExcelException("Cell data type not supported");
        } 

        private static Cell Create(DateTime data)
        {
            var cell = new Cell
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(data.ToOADate().ToString()),
                StyleIndex = SpreadSheetStyleSheet.GetDateCellFormat()
            };
            return cell;
        }

        private static Cell Create(int? data)
        {
            var cell = new Cell
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(data ?? 0),
                StyleIndex = SpreadSheetStyleSheet.GetCenteredCellFormat()
            };
            return cell;
        }

        private static Cell Create(double? data)
        {
            var cell = new Cell
            {
                DataType = CellValues.Number,
                CellValue = new CellValue(data ?? 0),
                StyleIndex = SpreadSheetStyleSheet.GetCenteredCellFormat()
            };
            return cell;
        }

        private Cell Create(string? data)
        {
            var cell = new Cell
            {
                DataType = CellValues.SharedString,
                CellValue = new CellValue(InsertSharedStringItem(data ?? string.Empty).ToString()),
                StyleIndex = SpreadSheetStyleSheet.GetCenteredCellFormat(),
            };
            return cell;
        }

        private static Cell Create(bool data)
        {
            var cell = new Cell
            {
                DataType = CellValues.Boolean,
                CellValue = new CellValue(data),
                StyleIndex = SpreadSheetStyleSheet.GetCenteredCellFormat(),
            };
            return cell;
        }

        private string? ReadString(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) 
            {
                return null;
            }

            return sharedStringTablePart.SharedStringTable
                        .ElementAt(int.Parse(value))
                        .InnerText;
        }

        private static DateTime? ReadDate(string? value)
        {
            var dateTime = new DateTime(1899, 12, 31);

            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return dateTime.AddDays(int.Parse(value));
        }

        private static int? ReadInt(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return int.Parse(value);
        }

        private static double? ReadDouble(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return double.Parse(value, CultureInfo.InvariantCulture);
        }

        private bool CheckForDateFormat(Cell cell)
        {
            if (cell.StyleIndex == null)
            {
                throw new ExcelException("Cell style not found");
            }

            uint numberFormatId = (stylesheet.CellFormats!.ElementAt((int)(uint)cell.StyleIndex) as CellFormat)!.NumberFormatId ?? throw new ExcelException("Cells number format not found");

            return numberFormatId switch
            {
                14 or 15 or 16 or 17 or 22 or 27 or 30 or 36 or 50 or 57 => true,
                _ => false,
            };
        }

        private int InsertSharedStringItem(string text)
        {
            var sharedStringTable = sharedStringTablePart.SharedStringTable;
            int index = 0;

            foreach (var item in sharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText.Equals(text))
                {
                    return index;
                }
                index++;
            }

            sharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
            return index;
        }
    }
}
