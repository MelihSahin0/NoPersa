using System.ComponentModel.DataAnnotations;
using Website.Client.Components.Default;
using Website.Client.Models;
using Website.Client.Util;

namespace Website.Client.FormModels
{
    public class BoxStatusModel
    {
        [Required]
        public required List<BoxStatusOverview> BoxStatusOverviews { get; set; }

        public List<SelectInput> DefaultNumbers = Misc.GetDefaultNumberOfBoxesSelection;

        public string RouteFilter { get; set; } = string.Empty;
       
        public string CustomerFilter { get; set; } = string.Empty;
    }
}
