using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using Website.Client.Components.Default;

namespace Website.Client.FormModels
{
    public class BoxConfigurationModel
    {
        [ValidateComplexType]
        [Required]
        public required List<DragDropInput<string>> LightDiets { get; set; }

        [ValidateComplexType]
        [Required]
        [MinChildren(1, ErrorMessage = "This requires one box content")]
        public required List<DragDropInput<string>> BoxContents { get; set; }

        [ValidateComplexType]
        [Required]
        [MinChildren(1, ErrorMessage = "This requires one portion size")]
        public required List<DragDropInput<string>> PortionSizes { get; set; }
    }
}
