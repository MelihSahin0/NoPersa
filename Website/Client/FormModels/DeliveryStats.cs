using System.ComponentModel.DataAnnotations;
using Website.Client.Models;

namespace Website.Client.FormModels
{
    public class DeliveryStats
    {
        [ValidateComplexType]
        [Required]
        public required List<RouteDetails> RouteDetails { get; set; }
    }
}
