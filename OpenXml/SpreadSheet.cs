using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using OpenXml.Exceptions;
using OpenXml.Models;
using Table = OpenXml.Models.Table;

namespace OpenXml
{
    public static class SpreadSheet
    {
        public static byte[] CreateSpreadsheet(List<Table> tables)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var spreadsheetDocument = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
                {
                    var workbookPart = spreadsheetDocument.AddWorkbookPart();
                    workbookPart.Workbook = new Workbook();

                    var sharedStringTablePart = workbookPart.AddNewPart<SharedStringTablePart>();
                    sharedStringTablePart.SharedStringTable = new SharedStringTable();

                    Stylesheet stylesheet = new SpreadSheetStyleSheet().Get();
                    workbookPart.AddNewPart<WorkbookStylesPart>();
                    workbookPart.WorkbookStylesPart!.Stylesheet = stylesheet;

                    var sheets = spreadsheetDocument.WorkbookPart!.Workbook.AppendChild(new Sheets());
                    uint sheetId = 1;

                    IdGenerator idGenerator = new(1);

                    foreach (var table in tables)
                    {
                        var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                        var sheetData = new SheetData();
                        worksheetPart.Worksheet = new Worksheet(sheetData);

                        var sheet = new Sheet()
                        {
                            Id = spreadsheetDocument.WorkbookPart.GetIdOfPart(worksheetPart),
                            SheetId = sheetId++,
                            Name = table.Name
                        };
                        sheets.Append(sheet);

                        var cellFactory = new SpreadSheetCell(sharedStringTablePart, stylesheet);
                        var rowFactory = new SpreadSheetRow(sheetData, cellFactory);
                        var tableFactory = new SpreadSheetTable(worksheetPart, idGenerator);
                        var columnFactory = new SpreadSheetColumn(worksheetPart, sheetData);
       
                        rowFactory.CreateHeader(table.Headers);
                        rowFactory.CreateData(table.Data);
                        tableFactory.Create(table.Headers, table.Data.Count == 0 ? 1 : table.Data.Count);
                        columnFactory.Create(table.Headers.Count, 20);
                        
                        if (table.DropDownInformations.Count != 0)
                        {
                            var dataValidationFactory = new SpreadSheetDataValidation(worksheetPart, sheetData);
                            dataValidationFactory.Create(table.DropDownInformations);
                        }

                        worksheetPart.Worksheet.Save();
                    }

                    workbookPart.Workbook.Save();
                }

                return memoryStream.ToArray();
            }
        }

        public static List<Table> ReadSpreadSheet(byte[] excelData, bool allTables = true, int maxNumberOfRow = 0)
        {
            using (var memoryStream = new MemoryStream(excelData))
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Open(memoryStream, false))
                {
                    List<Table> table = [];
 
                    WorkbookPart workbookPart = document.WorkbookPart!;
                    SharedStringTablePart sharedStringTablePart = workbookPart.SharedStringTablePart ?? throw new ExcelException("SharedStringTable not found");
                    Stylesheet stylesPart = workbookPart.WorkbookStylesPart?.Stylesheet ?? throw new ExcelException("StyleSheet not found");

                    List<Sheet> sheets = allTables ? workbookPart.Workbook.Descendants<Sheet>().ToList() : [ workbookPart.Workbook.Descendants<Sheet>().FirstOrDefault()];
                    if (sheets.Count == 0)
                    {
                        throw new ExcelException("No sheets found in the workbook.");
                    }

                    foreach (var sheet in sheets)
                    {
                        List<string> headers = [];
                        List<List<object?>> data = [];

                        WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet.Id!);
                        TableDefinitionPart? tablePart = worksheetPart.GetPartsOfType<TableDefinitionPart>().FirstOrDefault(tdp => 
                        {
                            string tableName = tdp.Table.Name?.ToString() ?? string.Empty;
                            return tableName.StartsWith(SpreadSheetTable.TableName);
                        }) ?? throw new ExcelException("No tables found in the sheet.");
                        
                        string[] rangeParts = SpreadSheetTable.ReadMetaData(tablePart);

                        SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>() ?? throw new ExcelException("SheetData not found"); ;
                        var cellFactory = new SpreadSheetCell(sharedStringTablePart, stylesPart);
                        var rowFactory = new SpreadSheetRow(sheetData, cellFactory);

                        headers = rowFactory.ReadHeader(rangeParts[0]);
                        data = rowFactory.ReadData(rangeParts[0], rangeParts[1], maxNumberOfRow);

                        table.Add(new() { Name = $"{sheet.Name}", Headers = headers, Data = data });
                    }

                    return table;
                }
            }
        }
    }
}
