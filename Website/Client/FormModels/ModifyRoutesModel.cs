using System.ComponentModel.DataAnnotations;
using Website.Client.Models;

namespace Website.Client.FormModels
{
    public class ModifyRoutesModel
    {
        [ValidateComplexType]
        [Required]
        public required List<RouteSummary> RouteSummary { get; set; }
    }
}
