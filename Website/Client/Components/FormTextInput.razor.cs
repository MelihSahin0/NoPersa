using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Linq.Expressions;
using Website.Client.Styles;

namespace Website.Client.Components
{
    public partial class FormTextInput
    {
        private readonly string Id = Guid.NewGuid().ToString();

        [Parameter]
        public string LabelOrientation { get; set; } = "top";

        [Parameter]
        public string LabelTextOrientation { get; set; } = "left";

        [Parameter]
        public string TextOrientation { get; set; } = "text-left";

        [Parameter]
        public bool IsDisabled { get; set; } = false;

        [Parameter]
        public string? Placeholder { get; set; }

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

        public string ValidStateCss()
        {
            if (For == null)
            {
                InputStyles.GetBorderDefaultStyle(false);
            }

            var fieldIdentifier = FieldIdentifier.Create(For);
            var isInvalid = CascadedEditContext!.GetValidationMessages(fieldIdentifier).Any();

            return InputStyles.GetBorderDefaultStyle(isInvalid);
        }
    }
}