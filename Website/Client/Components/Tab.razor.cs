using Microsoft.AspNetCore.Components;

namespace Website.Client.Components
{
    public partial class Tab
    {
        [Parameter]
        public required int Value { get; set; }

        [Parameter]
        public required List<string> Items { get; set; }

        [Parameter]
        public EventCallback<int> ValueChanged { get; set; }

        private async Task OnButtonClick(int index)
        {
            Value = index;
            await ValueChanged.InvokeAsync(Value);
        }
    }
}
