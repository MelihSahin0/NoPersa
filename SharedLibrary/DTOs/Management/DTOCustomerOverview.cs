using SharedLibrary.DTOs.GetDTOs;

namespace SharedLibrary.DTOs.Management
{
    public class DTOCustomerOverview
    {
        public int Id { get; set; }

        public string? SerialNumber { get; set; }

        public string? Title { get; set; }

        public string? Name { get; set; }

        public DTODeliveryLocation? DeliveryLocation { get; set; }

        public string? ContactInformation { get; set; }

        public bool TemporaryDelivery { get; set; }

        public bool TemporaryNoDelivery { get; set; }

        public DTOWeekdays? Workdays { get; set; }

        public DTOWeekdays? Holidays { get; set; }

        public DTOMonthOfTheYear? DisplayMonth { get; set; }

        public int? ArticleId { get; set; }

        public string? DefaultNumberOfBoxes { get; set; }

        public DTOMonthlyDelivery[]? MonthlyDeliveries { get; set; }

        public int? RouteId { get; set; }

        public List<DTOLightDietOverview>? LightDietOverviews { get; set; }

        public List<DTOBoxContentSelected>? BoxContentSelectedList { get; set; }

        public List<DTOSelectInput>? PortionSizes { get; set; }
    }
}
