using OpenXml.Models;
using System.ComponentModel.DataAnnotations;
using Website.Client.Enums;

namespace Website.Client.FormModels
{
    public class InvoiceExportModel
    {
        public required List<string> SheetNames { get; set; }

        public required List<string> Header { get; set; }

        public required List<List<Invoice>> Invoices { get; set; }

        [Required]
        public required bool SplitToMultipleRoutes { get; set; }

        [Required]
        public required bool ShowAllDays { get; set; }

        [Required]
        public required int Year { get; set; }

        [Required]
        public required Months Month { get; set; }

        public byte[] ExcelFileBytes { get; set; } = [];
    }
}
