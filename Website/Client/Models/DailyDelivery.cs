using SharedLibrary.Validations;

namespace Website.Client.Models
{
    public class DailyDelivery
    {
        [DoubleType(min: 0)]
        public string? Price { get; set; }

        [IntType(min: 0)]
        public string? NumberOfBoxes { get; set; }
    }
}
