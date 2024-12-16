namespace NoPersaService.DTOs.Management.RA
{
    public class DTOMonthlyDelivery
    {
        public DTOMonthOfTheYear? MonthOfTheYear { get; init; }
        public List<DTODailyDelivery>? DailyDeliveries { get; set; }
    }
}
