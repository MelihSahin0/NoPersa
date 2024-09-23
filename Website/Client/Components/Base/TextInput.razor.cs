using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Website.Client.Components.Base
{
    public partial class TextInput : InputBase<string?>
    {
        private string? previousValidValue;

        [Parameter]
        public string? Placeholder { get; set; }

        [Parameter]
        public Func<string?, bool>? ValidationFunction { get; set; }

        protected override bool TryParseValueFromString(string? value, out string? result, out string validationErrorMessage)
        {
            validationErrorMessage = "";

            if (ValidationFunction != null && !ValidationFunction(value))
            {
                CurrentValueAsString = previousValidValue;
                result = previousValidValue;
                return true;
            }

            result = value;
            previousValidValue = result;
            return true;
        }

        protected override string FormatValueAsString(string? value)
        {
            return value?.ToString() ?? "";
        }

        protected override void OnInitialized()
        {
            previousValidValue = Value;
        }
    }
}
