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
        public required long[] SelectedRouteDetailsId { get; set; }

        [Required]
        public required List<SelectInput> RouteOverview { get; set; }
    }
}
