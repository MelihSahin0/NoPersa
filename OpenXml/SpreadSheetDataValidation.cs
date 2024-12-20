using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.ComponentModel;
using static OpenXml.Models.Table;

namespace OpenXml
{
    public class SpreadSheetDataValidation
    {
        private readonly WorksheetPart worksheetPart;
        private readonly SheetData sheetData;

        public SpreadSheetDataValidation(WorksheetPart worksheetPart, SheetData sheetData)
        {
            this.worksheetPart = worksheetPart;
            this.sheetData = sheetData;
        }

        public void Create(List<DropDownInformation> dropDownInformations)
        {
            DataValidations dataValidations = worksheetPart.Worksheet.GetFirstChild<DataValidations>() ?? new DataValidations();

            foreach (DropDownInformation dropDownInformation in dropDownInformations)
            {
                DataValidation dataValidation = new()
                {
                    Type = DataValidationValues.List,
                    AllowBlank = dropDownInformation.AllowBlank,
                    ShowInputMessage = true,
                    ShowErrorMessage = true,
                    SequenceOfReferences = new ListValue<StringValue>() { InnerText = $"{dropDownInformation.Column}{dropDownInformation.StartRow}:{dropDownInformation.Column}{dropDownInformation.EndRow}" },
                    Formula1 = new Formula1($"\"{string.Join(",", dropDownInformation.Data)}\"")
                };
                dataValidations.Append(dataValidation);
            }

            if (worksheetPart.Worksheet.GetFirstChild<DataValidations>() == null)
            {
                worksheetPart.Worksheet.InsertAfter(dataValidations, sheetData);
            }
        }
    }
}
