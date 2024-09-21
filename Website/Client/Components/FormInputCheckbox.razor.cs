using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Website.Client.Components
{
    public partial class FormInputCheckbox
    {
        private readonly string Id = Guid.NewGuid().ToString();

        [CascadingParameter]
        private EditContext? CascadedEditContext { get; set; }

        [Parameter]
        public required string Label { get; init; }

        [Parameter]
        public bool Value { get; set; }

        [Parameter]
        public EventCallback<bool> ValueChanged { get; set; }

        private async Task OnValueChanged(ChangeEventArgs e)
        {
            var newValue = (bool?)Convert.ChangeType(e.Value, typeof(bool));
            Value = newValue ?? false;
            await ValueChanged.InvokeAsync(Value);
        }
    }
}
