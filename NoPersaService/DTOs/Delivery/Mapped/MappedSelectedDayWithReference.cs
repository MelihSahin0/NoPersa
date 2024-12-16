namespace NoPersaService.DTOs.Delivery.Mapped
{
    public class MappedSelectedDayWithReference
    {
        public long ReferenceId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int Day { get; set; }

        public string? GeoLocation { get; set; }
    }
}
