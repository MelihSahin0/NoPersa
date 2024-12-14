using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Components.Default
{
    public partial class FormDefaultSelect
    {
        private readonly string Id = Guid.NewGuid().ToString();

        [Parameter]
        public required string Class { get; init; }

        [Parameter]
        public bool IsColumn { get; set; } = false;

        [Parameter]
        public required string Label { get; init; }

        [Parameter]
        public List<SelectInput>? SelectInputs { get; set; }

        [Parameter]
        public long Value { get; set; }

        [Parameter]
        public EventCallback<long> ValueChanged { get; set; }

        private List<SelectInput>? previousSelectInputs;

        protected override async Task OnParametersSetAsync()
        {
            if (!ListComparer(previousSelectInputs, SelectInputs))
            {
                previousSelectInputs = SelectInputs?.Select(x => new SelectInput() { Id = x.Id, Value = x.Value }).ToList();
                await ValueChanged.InvokeAsync(Value);
            }
        }

        private static bool ListComparer(List<SelectInput>? oldList, List<SelectInput>? newList)
        {
            if (oldList == null || newList == null) return false;
            if (oldList.Count != newList.Count) return false;
            return !oldList.Where((t, i) => t.Id != newList[i].Id || t.Value != newList[i].Value).Any();
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

    public class SelectInput
    {
        [Required]
        public required long Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public required string Value { get; set; }
    }
}
