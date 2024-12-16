using System.ComponentModel.DataAnnotations;
using Website.Client.Components.Default;
using Website.Client.Models;

namespace Website.Client.FormModels
{
    public class CustomerSequenceModel
    {
        [Required]
        public required List<SequenceDetail> SequenceDetails { get; set; }

        [Required]
        public required string[] SelectedRouteDetailsId { get; set; }

        [Required]
        public required List<SelectInput<string>> RouteOverview { get; set; }
    }
}
