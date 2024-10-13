using SharedLibrary.Validations;

namespace Website.Client.Models
{
    public class MonthlyDelivery
    {
        public required MonthOfTheYear MonthOfTheYear { get; set; }

        [ExactChildren(31)]
        public required List<DailyDelivery> DailyDeliveries { get; set; }
    }
}
