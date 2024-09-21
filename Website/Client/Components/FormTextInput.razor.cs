using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;

namespace Website.Client.Components
{
    public partial class FormTextInput
    {
        private readonly string Id = Guid.NewGuid().ToString();

        [CascadingParameter]
        private EditContext? CascadedEditContext { get; set; }

        [Parameter]
        public Func<string?, bool>? ValidationFunction { get; set; }

        [Parameter]
        public string? Class { get; init; }

        [Parameter]
        public required string Label { get; init; }

        [Parameter]
        public string? Value { get; set; }

        [Parameter]
        public EventCallback<string?> ValueChanged { get; set; }

        [Parameter]
        public Expression<Func<string?>>? ValueExpression { get; set; }

        [Parameter]
        public Expression<Func<string>>? For { get; set; }

        private async Task OnValueInput(ChangeEventArgs e)
        {
            var newValue = (string?)Convert.ChangeType(e.Value, typeof(string));
            Value = newValue;
            await ValueChanged.InvokeAsync(Value);
            CascadedEditContext?.NotifyFieldChanged(FieldIdentifier.Create(For));
        }

        public string ValidStateCss()
        {
            if (For == null) return "border-black";

            var fieldIdentifier = FieldIdentifier.Create(For);
            var isInvalid = CascadedEditContext!.GetValidationMessages(fieldIdentifier).Any();

            return isInvalid ? "border-red" : "border-black";
        }
    }
}