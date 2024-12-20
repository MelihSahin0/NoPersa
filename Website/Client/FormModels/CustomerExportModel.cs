using OpenXml.Models;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.FormModels
{
    public class CustomerExportModel
    {
        public required List<string> SheetNames { get; set; }

        public required List<string> Header { get; set; }

        public required List<List<ExcelCustomer>> Customers { get; set; }

        [Required]
        public required bool SplitToMultipleRoutes { get; set; }

        public byte[] ExcelFileBytes { get; set; } = [];
    }
}
