﻿using NoPersaService.DTOs.General.Answer;

namespace NoPersaService.DTOs.Management.RA
{
    public class DTOCustomerOverview
    {
        public string? Id { get; set; }

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

        public string? ArticleId { get; set; }

        public int DefaultNumberOfBoxes { get; set; }

        public DTOMonthlyDelivery[]? MonthlyDeliveries { get; set; }

        public string? RouteId { get; set; }

        public List<DTOLightDietOverview>? LightDietOverviews { get; set; }

        public List<DTOBoxContentSelected>? BoxContentSelectedList { get; set; }

        public List<DTOSelectInput>? PortionSizes { get; set; }

        public List<DTOFoodWishesOverview>? FoodWishesOverviews { get; set; }

        public List<DTOFoodWishesOverview>? IngredientWishesOverviews { get; set; }
    }
}
