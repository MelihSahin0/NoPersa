using SharedLibrary.DTOs.GetDTOs;

namespace SharedLibrary.DTOs.Management
{
    public class DTOMonthlyDelivery
    {
        public DTOMonthOfTheYear? MonthOfTheYear { get; init; }
        public List<DTODailyDelivery>? DailyDeliveries { get; set; }
    }
}
