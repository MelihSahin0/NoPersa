
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Website.Client.Models;

namespace Website.Client.Components
{
    public partial class FormInputSelect
    {
        [CascadingParameter]
        private EditContext? CascadedEditContext { get; set; }

        [Parameter]
        public required string Label { get; init; }

        [Parameter]
        public int? Value { get; set; }

        [Parameter]
        public List<Route>? List { get; set; }

        [Parameter]
        public EventCallback<int?> ValueChanged { get; set; }

        private async Task OnValueChanged(ChangeEventArgs e)
        {
            if (e.Value?.ToString() == "null")
            {
                Value = null;
            }
            else
            {
                if (int.TryParse(e.Value?.ToString(), out int newValue))
                {
                    Value = newValue;
                }
                else
                {
                    Value = null;
                }
            }
            await ValueChanged.InvokeAsync(Value);
        }
    }
}
