using DocumentFormat.OpenXml.Spreadsheet;

namespace OpenXml
{
    public class SpreadSheetStyleSheet
    {
        private readonly Stylesheet stylesheet;

        public SpreadSheetStyleSheet()
        {
            stylesheet = new()
            {
                Fonts = new Fonts(new Font()),
                Fills = new Fills(new Fill()),
                Borders = new Borders(new Border()),
                CellStyleFormats = new CellStyleFormats(new CellFormat()),
                CellFormats = new CellFormats(new CellFormat())
            };
        }

        public Stylesheet Get()
        {
            stylesheet.CellFormats!.Append(CreateCenteredCellFormat());
            stylesheet.CellFormats.Append(CreateDateCellFormat());
            return stylesheet;
        }

        private static CellFormat CreateCenteredCellFormat()
        {
            var cellFormat = new CellFormat()
            {
                Alignment = new Alignment()
                {
                    Horizontal = HorizontalAlignmentValues.Center,
                    Vertical = VerticalAlignmentValues.Center
                },
                NumberFormatId = 0
            };

            return cellFormat;
        }

        private static CellFormat CreateDateCellFormat()
        {
            var cellFormat = new CellFormat()
            {
                Alignment = new Alignment()
                {
                    Horizontal = HorizontalAlignmentValues.Center,
                    Vertical = VerticalAlignmentValues.Center
                },
                NumberFormatId = 14,
                ApplyNumberFormat = true
            };

            return cellFormat;
        }

        public static uint GetCenteredCellFormat() => 1;
        public static uint GetDateCellFormat() => 2;
    }
}
