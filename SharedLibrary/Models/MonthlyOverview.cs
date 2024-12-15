using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public class MonthlyOverview
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required int Year { get; set; }

        [Required]
        public required int Month { get; set; }

        public List<DailyOverview> DailyOverviews { get; set; } = [];

        [Required]
        [ForeignKey("CustomerId")]
        public required long CustomerId { get; set; }

        public Customer? Customer { get; set; }

        [NotMapped]
        public double TotalPrice => CalculateTotalPrice();

        [NotMapped]
        public int TotalBoxes => CalculateTotalBoxes();

        [NotMapped]
        public double TotalSales => TotalPrice * TotalBoxes;

        private double CalculateTotalPrice()
        {
            double total = 0;
            foreach (var dailyOverview in DailyOverviews)
            {
                total += dailyOverview.Price ?? 0;
            }
            return total;
        }

        private int CalculateTotalBoxes()
        {
            int total = 0;
            foreach (var dailyOverview in DailyOverviews)
            {
                total += dailyOverview.NumberOfBoxes ?? 0;
            }
            return total;
        }
    }
}
