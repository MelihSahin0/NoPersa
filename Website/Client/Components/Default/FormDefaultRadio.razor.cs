using Microsoft.AspNetCore.Components;

namespace Website.Client.Components.Default
{
    public partial class FormDefaultRadio
    {
        [Parameter]
        public required string Class { get; set; } = string.Empty;

        [Parameter]
        public required bool Value { get; set; }

        [Parameter]
        public EventCallback<bool> ValueChanged { get; set; }

        [Parameter]
        public EventCallback SelectedValue { get; set; }

        private async Task OnValueChanged(ChangeEventArgs e)
        {
            bool value = (bool)e.Value!;
            Value = true;
            await ValueChanged.InvokeAsync(Value);
            if (value)
            {
                await SelectedValue.InvokeAsync();
            }
        }
    }
}
