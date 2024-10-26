using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class MonthlyDelivery
    {
        [Required]
        public required MonthOfTheYear MonthOfTheYear { get; set; }

        [Required]
        [ExactChildren(31)]
        public required List<DailyDelivery> DailyDeliveries { get; set; }
    }
}
