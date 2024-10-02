using System.ComponentModel.DataAnnotations;

namespace Website.Client.FormModels
{
    public class Route
    {
        [ValidateComplexType]
        [Required]
        public required List<Models.Route> Routes { get; set; }
    }
}
