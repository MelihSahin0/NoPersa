using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace OpenXml
{
    public class SpreadSheetTable
    {
        public static readonly string TableName = "NoPersa";
        private readonly WorksheetPart worksheetPart;
        private readonly IdGenerator idGenerator;

        public SpreadSheetTable(WorksheetPart worksheetPart, IdGenerator idGenerator)
        {
            this.worksheetPart = worksheetPart;
            this.idGenerator = idGenerator; 
        }

        public void Create(List<string> header, int numberOfData)
        {
            var id = idGenerator.GetId();
            string tableName = $"{TableName}{id}";
            var tableDefinitionPartId = "rId" + id;
            var tableDefinitionPart = worksheetPart.AddNewPart<TableDefinitionPart>(tableDefinitionPartId);
            var reference = "A1:" + GetExcelColumnName(header.Count) + (numberOfData + 1);
            tableDefinitionPart.Table = new DocumentFormat.OpenXml.Spreadsheet.Table()
            {
                Id = (uint)id,
                Name = tableName,
                DisplayName = tableName,
                Reference = reference,
                TotalsRowShown = false,
                AutoFilter = new AutoFilter()
                {
                    Reference = reference
                },
                TableColumns = new TableColumns()
                {
                    Count = (uint)header.Count
                },
                TableStyleInfo = new TableStyleInfo()
                {
                    Name = "TableStyleLight9",
                    ShowFirstColumn = false,
                    ShowLastColumn = false,
                    ShowRowStripes = true,
                    ShowColumnStripes = true
                }
            };

            for (int i = 0; i < header.Count; i++)
            {
                tableDefinitionPart.Table.TableColumns!.Append(new TableColumn()
                {
                    Id = (uint)(i + 1),
                    Name = header[i],
                });
            }

            TableParts? tableParts = (TableParts?)worksheetPart.Worksheet.ChildElements.FirstOrDefault(ce => ce is TableParts);
            if (tableParts == null)
            {
                tableParts = new()
                {
                    Count = (uint)0
                };
                worksheetPart.Worksheet.Append(tableParts);
            }
            tableParts.Count = tableParts.Count! + (uint)1;
            TablePart tablePart = new() { Id = tableDefinitionPartId};

            tableParts.Append(tablePart);
        }

        /// <summary>
        /// A = 0 and Z = 26 ect..
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <returns></returns>
        public static string GetExcelColumnName(int columnIndex)
        {
            string columnName = string.Empty;
            while (columnIndex > 0)
            {
                columnIndex--;
                columnName = (char)(columnIndex % 26 + 'A') + columnName;
                columnIndex /= 26;
            }
            return columnName;
        }

        public static string[] ReadMetaData(TableDefinitionPart tableDefinitionPart)
        {
            var tableRange = tableDefinitionPart.Table.Reference!.Value!;
            return tableRange!.Split(':');
        }
    }
}
