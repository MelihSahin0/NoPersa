using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public class DailyOverview
    {
        [Key]
        public int Id { get; set; }

        [DoubleType(min: 0)]
        public double? Price { get; set; }

        [IntType(min: 0)]
        public int? NumberOfBoxes { get; set; }

        [NotMapped]
        public double TotalSales => (Price ?? 0) * (NumberOfBoxes ?? 0);
    }
}
