using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace OpenXml
{
    public class SpreadSheetColumn
    {
        private readonly WorksheetPart worksheetPart;
        private readonly SheetData sheetData;

        public SpreadSheetColumn(WorksheetPart worksheetPart, SheetData sheetData)
        {
            this.worksheetPart = worksheetPart;
            this.sheetData = sheetData;
        }

        public void Create(int numberOfColumns, double width)
        {
            var columns = sheetData.Parent!.Elements<Columns>().FirstOrDefault();
            columns ??= new Columns();
            columns.RemoveAllChildren();

            for (int colIndex = 0; colIndex < numberOfColumns; colIndex++)
            {
                columns.Append(new Column()
                {
                    Min = (uint)(colIndex + 1),
                    Max = (uint)(colIndex + 1),
                    Width = width,
                    CustomWidth = true
                });
            }

            worksheetPart.Worksheet.InsertBefore(columns, sheetData);
        }
    }
}
