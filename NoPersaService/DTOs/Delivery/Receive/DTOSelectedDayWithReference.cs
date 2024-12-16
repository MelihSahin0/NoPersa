namespace NoPersaService.DTOs.Delivery.Receive
{
    public class DTOSelectedDayWithReference
    {
        public string? ReferenceId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int Day { get; set; }

        public string? GeoLocation { get; set; }
    }
}
