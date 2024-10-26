using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
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
