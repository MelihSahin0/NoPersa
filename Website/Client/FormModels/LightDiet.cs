using System.ComponentModel.DataAnnotations;
using Website.Client.Components.Default;

namespace Website.Client.FormModels
{
    public class LightDiet
    {
        [ValidateComplexType]
        [Required]
        public required List<DragDropInput> DragDropInputs { get; set; }
    }
}
