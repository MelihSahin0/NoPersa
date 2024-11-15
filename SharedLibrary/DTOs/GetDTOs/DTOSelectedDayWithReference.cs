namespace SharedLibrary.DTOs.GetDTOs
{
    public class DTOSelectedDayWithReference
    {
        public int ReferenceId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public int Day { get; set; }

        public string? GeoLocation { get; set; }
    }
}
