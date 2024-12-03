using Microsoft.AspNetCore.Components;

namespace Website.Client.Components
{
    public partial class FormInputRadio
    {
        private readonly string Id1 = Guid.NewGuid().ToString();
        private readonly string Id2 = Guid.NewGuid().ToString();

        [Parameter]
        public required string Label1 { get; init; }

        [Parameter]
        public required string Label2 { get; init; }

        [Parameter]
        public required bool Value1 { get; set; }

        [Parameter]
        public EventCallback<bool> Value1Changed { get; set; }

        [Parameter]
        public required bool Value2 { get; set; }

        [Parameter]
        public EventCallback<bool> Value2Changed { get; set; }

        private async Task OnValueChanged(ChangeEventArgs e, int index)
        {
            if (index == 1)
            {
                Value1 = (bool)e.Value!;

                if (Value1)
                {
                    Value2 = false;
                    await Value2Changed.InvokeAsync(Value2);
                }

                await Value1Changed.InvokeAsync(Value1);
            }
            else
            {
                Value2 = (bool)e.Value!;

                if (Value2)
                {
                    Value1 = false;
                    await Value1Changed.InvokeAsync(Value1);
                }

                await Value2Changed.InvokeAsync(Value2);
            }
        }
    }
}
