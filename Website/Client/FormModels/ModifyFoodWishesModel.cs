using System.ComponentModel.DataAnnotations;
using Website.Client.Components.Default;

namespace Website.Client.FormModels
{
    public class ModifyFoodWishesModel
    {
        [ValidateComplexType]
        [Required]
        public required List<DragDropInput<string>> FoodWishes { get; set; }

        [ValidateComplexType]
        [Required]
        public required List<DragDropInput<string>> IngredientWishes { get; set; }
    }
}
