using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public class MonthlyOverview
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [IntType(min: 0, max: 11)]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        [ExactChildren(31)]
        public List<DailyOverview> DailyOverviews { get; set; } = [];

        [ForeignKey("CustomerId")]
        public int CustomerId { get; set; }

        public required Customer Customer { get; set; }

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
