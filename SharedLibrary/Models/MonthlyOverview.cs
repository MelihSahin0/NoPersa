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

        [ForeignKey("Day1Id")]
        public int Day1Id { get; set; }

        [Required]
        public required DailyOverview Day1 { get; set; }

        [ForeignKey("Day2Id")]
        public int Day2Id { get; set; }

        [Required]
        public required DailyOverview Day2 { get; set; }

        [ForeignKey("Day3Id")]
        public int Day3Id { get; set; }

        [Required]
        public required DailyOverview Day3 { get; set; }

        [ForeignKey("Day4Id")]
        public int Day4Id { get; set; }

        [Required]
        public required DailyOverview Day4 { get; set; }

        [ForeignKey("Day5Id")]
        public int Day5Id { get; set; }

        [Required]
        public required DailyOverview Day5 { get; set; }

        [ForeignKey("Day6Id")]
        public int Day6Id { get; set; }

        [Required]
        public required DailyOverview Day6 { get; set; }

        [ForeignKey("Day7Id")]
        public int Day7Id { get; set; }

        [Required]
        public required DailyOverview Day7 { get; set; }

        [ForeignKey("Day8Id")]
        public int Day8Id { get; set; }

        [Required]
        public required DailyOverview Day8 { get; set; }

        [ForeignKey("Day9Id")]
        public int Day9Id { get; set; }

        [Required]
        public required DailyOverview Day9 { get; set; }

        [ForeignKey("Day10Id")]
        public int Day10Id { get; set; }

        [Required]
        public required DailyOverview Day10 { get; set; }

        [ForeignKey("Day11Id")]
        public int Day11Id { get; set; }

        [Required]
        public required DailyOverview Day11 { get; set; }

        [ForeignKey("Day12Id")]
        public int Day12Id { get; set; }

        [Required]
        public required DailyOverview Day12 { get; set; }

        [ForeignKey("Day13Id")]
        public int Day13Id { get; set; }

        [Required]
        public required DailyOverview Day13 { get; set; }

        [ForeignKey("Day14Id")]
        public int Day14Id { get; set; }

        [Required]
        public required DailyOverview Day14 { get; set; }

        [ForeignKey("Day15Id")]
        public int Day15Id { get; set; }

        [Required]
        public required DailyOverview Day15 { get; set; }

        [ForeignKey("Day16Id")]
        public int Day16Id { get; set; }

        [Required]
        public required DailyOverview Day16 { get; set; }

        [ForeignKey("Day17Id")]
        public int Day17Id { get; set; }

        [Required]
        public required DailyOverview Day17 { get; set; }

        [ForeignKey("Day18Id")]
        public int Day18Id { get; set; }

        [Required]
        public required DailyOverview Day18 { get; set; }

        [ForeignKey("Day19Id")]
        public int Day19Id { get; set; }

        [Required]
        public required DailyOverview Day19 { get; set; }

        [ForeignKey("Day20Id")]
        public int Day20Id { get; set; }

        [Required]
        public required DailyOverview Day20 { get; set; }

        [ForeignKey("Day21Id")]
        public int Day21Id { get; set; }

        [Required]
        public required DailyOverview Day21 { get; set; }

        [ForeignKey("Day22Id")]
        public int Day22Id { get; set; }

        [Required]
        public required DailyOverview Day22 { get; set; }

        [ForeignKey("Day23Id")]
        public int Day23Id { get; set; }

        [Required]
        public required DailyOverview Day23 { get; set; }

        [ForeignKey("Day24Id")]
        public int Day24Id { get; set; }

        [Required]
        public required DailyOverview Day24 { get; set; }

        [ForeignKey("Day25Id")]
        public int Day25Id { get; set; }

        [Required]
        public required DailyOverview Day25 { get; set; }

        [ForeignKey("Day26Id")]
        public int Day26Id { get; set; }

        [Required]
        public required DailyOverview Day26 { get; set; }

        [ForeignKey("Day27Id")]
        public int Day27Id { get; set; }

        [Required]
        public required DailyOverview Day27 { get; set; }

        [ForeignKey("Day28Id")]
        public int Day28Id { get; set; }

        [Required]
        public required DailyOverview Day28 { get; set; }

        [ForeignKey("Day29Id")]
        public int Day29Id { get; set; }

        [Required]
        public required DailyOverview Day29 { get; set; }

        [ForeignKey("Day30Id")]
        public int Day30Id { get; set; }

        [Required]
        public required DailyOverview Day30 { get; set; }

        [ForeignKey("Day31Id")]
        public int Day31Id { get; set; }

        [Required]
        public required DailyOverview Day31 { get; set; }


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
            for (int i = 1; i <= 31; i++)
            {
                if (this.GetType().GetProperty($"Day{i}")?.GetValue(this) is DailyOverview dailyOverview)
                {
                    total += dailyOverview.Price ?? 0;
                }
            }
            return total;
        }

        private int CalculateTotalBoxes()
        {
            int total = 0;
            for (int i = 1; i <= 31; i++)
            {
                if (this.GetType().GetProperty($"Day{i}")?.GetValue(this) is DailyOverview dailyOverview)
                {
                    total += dailyOverview.NumberOfBoxes ?? 0;
                }
            }
            return total;
        }
    }
}
