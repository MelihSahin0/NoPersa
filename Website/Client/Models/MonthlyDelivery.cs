using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using Website.Client.Enums;

namespace Website.Client.Models
{
    public class MonthlyDelivery
    {
        [Required]
        public required MonthOfTheYear MonthOfTheYear { get; set; }

        [Required]
        [MinChildren(28)]
        public required List<DailyDelivery> DailyDeliveries { get; set; }
    }

    public class MonthOfTheYear
    {
        [Required]
        public required Months Month { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Year { get; set; }
    }

    public class DailyDelivery
    {
        [Required]
        [IntType(min: 0)]
        public required int DayOfMonth { get; set; }

        [DoubleType(min: 0)]
        public string? Price { get; set; }

        [IntType(min: 0)]
        public string? NumberOfBoxes { get; set; }
    }
}
