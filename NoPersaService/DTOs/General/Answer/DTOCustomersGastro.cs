using NoPersaService.DTOs.Management.RA;

namespace NoPersaService.DTOs.General.Answer
{
    public class DTOCustomersGastro
    {
        public List<DTOLightDietOverview>? LightDietOverview { get; set; }

        public List<DTOFoodWishesOverview>? FoodWishesOverviews { get; set; }

        public List<DTOFoodWishesOverview>? IngredientWishesOverviews { get; set; }

        public List<DTOBoxContentSelected>? BoxContentSelectedList { get; set; }

        public List<DTOSelectInput>? SelectInputs { get; set; }
    }
}
