using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public class DailyOverview
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required int DayOfMonth { get; set; }

        public double? Price { get; set; }

        public int? NumberOfBoxes { get; set; }

        [Required]
        [ForeignKey("MonthlyOverviewId")]
        public required long MonthlyOverviewId { get; set; }

        public MonthlyOverview? MonthlyOverview { get; set; }

        [NotMapped]
        public double TotalSales => (Price ?? 0) * (NumberOfBoxes ?? 0);
    }
}
