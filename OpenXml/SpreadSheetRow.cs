using DocumentFormat.OpenXml.Spreadsheet;
using OpenXml.Exceptions;

namespace OpenXml
{
    public partial class SpreadSheetRow
    {
        private readonly SheetData sheetData;
        private readonly SpreadSheetCell spreadSheetCell;
        private readonly IdGenerator idGenerator;

        public SpreadSheetRow(SheetData sheetData, SpreadSheetCell spreadSheetCell)
        {
            this.sheetData = sheetData;
            this.spreadSheetCell = spreadSheetCell;
            this.idGenerator = new IdGenerator(1);
        }

        public void CreateHeader(List<string> headers)
        {
            Row row = new()
            {
                RowIndex = (uint)idGenerator.GetId()
            };
            foreach (var header in headers)
            {
                row.Append(spreadSheetCell.Create(header));
            }
            sheetData.Append(row);
        }

        public List<string> ReadHeader(string startRow)
        {
            List<string> headers = [];
            foreach (Cell cell in GetRow(startRow).Elements<Cell>())
            {
                headers.Add(spreadSheetCell.Read<string>(cell)!);
            }

            return headers;
        }

        public void CreateData(List<List<object?>> data)
        {
            foreach (var rowData in data)
            {
                Row row = new()
                {
                    RowIndex = (uint)idGenerator.GetId()
                };
                foreach (var cellData in rowData)
                {
                    row.Append(spreadSheetCell.Create(cellData));
                }
                sheetData.Append(row);
            }
        }

        public List<List<object?>> ReadData(string startRow, string endRow, int maxNumberOfRow)
        {
            List<List<object?>> data = [];
            foreach (Row row in GetRowsInRange(startRow, endRow, maxNumberOfRow))
            {
                List<object?> rowData = [];
                foreach (Cell cell in row.Elements<Cell>())
                {
                    rowData.Add(spreadSheetCell.Read<object?>(cell));
                }
                data.Add(rowData);
            }

            return data;
        }

        private Row GetRow(string startCell)
        {
            int rowIndex = int.Parse(System.Text.RegularExpressions.Regex.Match(startCell, @"\d+").Value);
            return sheetData.Elements<Row>().FirstOrDefault(r => 
            { 
                uint rowIndex = r.RowIndex ?? throw new ExcelException($"RowIndex not found.");
                return r.RowIndex == rowIndex;
            }) ?? throw new ExcelException($"Row not found: {rowIndex}.");
        }

        private IEnumerable<Row> GetRowsInRange(string startCell, string endCell, int maxNumberOfRow)
        {
            int startRow = int.Parse(NumberExtractor().Match(startCell).Value) + 1;
            int endRow = int.Parse(NumberExtractor().Match(endCell).Value);

            if (maxNumberOfRow > 0)
            {
                return sheetData.Elements<Row>().Where(r =>
                {
                    uint rowIndex = r.RowIndex ?? throw new ExcelException($"RowIndex not found.");
                    return rowIndex! >= startRow && rowIndex <= Math.Min(endRow, startRow + maxNumberOfRow - 1);
                });
            }
            else
            {
                return sheetData.Elements<Row>().Where(r =>
                {
                    uint rowIndex = r.RowIndex ?? throw new ExcelException($"RowIndex not found.");
                    return rowIndex >= startRow && rowIndex <= endRow;
                });
            }
        }

        [System.Text.RegularExpressions.GeneratedRegex(@"\d+")]
        private static partial System.Text.RegularExpressions.Regex NumberExtractor();
    }
}
