using System.ComponentModel.DataAnnotations;
using Website.Client.Components.Default;

namespace Website.Client.FormModels
{
    public class BoxConfigurationModel
    {
        [ValidateComplexType]
        [Required]
        public required List<DragDropInput> LightDiets { get; set; }

        [ValidateComplexType]
        [Required]
        public required List<DragDropInput> BoxContents { get; set; }

        [ValidateComplexType]
        [Required]
        public required List<DragDropInput> PortionSizes { get; set; }
    }
}
