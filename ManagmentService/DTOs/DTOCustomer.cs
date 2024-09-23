﻿
namespace ManagmentService.DTOs
{
    public class DTOCustomer
    {
        public int Id { get; set; }

        public string? SerialNumber { get; set; }

        public string? Title { get; set; }

        public string? Name { get; set; }

        public string? Address { get; set; }

        public string? Region { get; set; }

        public string? GeoLocation { get; set; }

        public string? ContactInformation { get; set; }

        public DTOWeekdays? Workdays { get; set; }

        public DTOWeekdays? Holidays { get; set; }

        public DTOMonthOfTheYear? DisplayMonth { get; set; }

        public string? Article { get; set; }

        public string? DefaultPrice { get; set; }

        public string? DefaultNumberOfBoxes { get; set; }

        public DTOMonthlyDelivery[]? MonthlyDeliveries { get; set; }
    }
}
