using Website.Client.Components;
using Website.Client.Components.Default;

namespace Website.Client.Models
{
    public class CustomersGastro
    {
        public List<SelectedInputCheckbox>? LightDietOverview { get; set; }

        public List<SelectedInputCheckbox>? FoodWishesOverviews { get; set; }

        public List<SelectedInputCheckbox>? IngredientWishesOverviews { get; set; }

        public List<BoxContentSelected>? BoxContentSelectedList { get; set; }

        public List<SelectInput>? SelectInputs { get; set; }
    }
}
