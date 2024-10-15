using Microsoft.AspNetCore.Components;
using Website.Client.Models;

namespace Website.Client.Components
{
    public partial class FormInputSelect
    {
        [Parameter]
        public required string Class { get; init; }

        [Parameter]
        public required string Label { get; init; }

        [Parameter]
        public int Value { get; set; }

        [Parameter]
        public List<RouteOverview>? RouteOverviews { get; set; }

        [Parameter]
        public EventCallback<int> ValueChanged { get; set; }

        private List<RouteOverview>? previousList;

        protected override async Task OnParametersSetAsync()
        {
            if (!ListComparer(previousList, RouteOverviews))
            {
                previousList = RouteOverviews?.Select(x => new RouteOverview() { Id = x.Id, Name = x.Name, Position = x.Position, NumberOfCustomers = x.NumberOfCustomers }).ToList(); 
                await ValueChanged.InvokeAsync(Value);
            }
        }

        private static bool ListComparer(List<RouteOverview>? oldList, List<RouteOverview>? newList)
        {
            if (oldList == null || newList == null) return false;
            if (oldList.Count != newList.Count) return false;
            return !oldList.Where((t, i) => t.Id != newList[i].Id || t.Name != newList[i].Name).Any();
        }

        private async Task OnValueChanged(ChangeEventArgs e)
        {
            if (int.TryParse(e.Value?.ToString(), out int newValue))
            {
                Value = newValue;
                await ValueChanged.InvokeAsync(Value);
            }
        }
    }
}
