using Website.Client.Components.Default;

namespace Website.Client.Models
{
    public class BoxContentOverview
    {
        public List<BoxContentSelected> BoxContentSelectedList { get; set; } = [];

        public List<SelectInput<string>> SelectInputs { get; set; } = [];
    }
}
