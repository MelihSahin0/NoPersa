using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace Website.Client.Components
{
    public partial class FormInputCheckbox
    {
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
