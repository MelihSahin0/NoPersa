﻿using Microsoft.AspNetCore.Components;

namespace Website.Client.Components
{
    public partial class FormInputCheckbox
    {
        private readonly string Id = Guid.NewGuid().ToString();

        [Parameter]
        public string Class { get; set; } = string.Empty;

        [Parameter]
        public required bool IsColumn { get; set; } = true;

        [Parameter]
        public required string Label { get; init; }

        [Parameter]
        public required bool Value { get; set; }

        [Parameter]
        public EventCallback<bool> ValueChanged { get; set; }

        [Parameter]
        public EventCallback<bool> OnValueChangedCallback { get; set; }


        private async Task OnValueChanged(ChangeEventArgs e)
        {
            var newValue = (bool?)Convert.ChangeType(e.Value, typeof(bool));
            Value = newValue ?? false;
            await ValueChanged.InvokeAsync(Value);
            await OnValueChangedCallback.InvokeAsync(Value);
        }
    }
}
