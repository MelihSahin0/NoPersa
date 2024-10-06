using Microsoft.AspNetCore.Components;

namespace Website.Client.Components
{
    public partial class Popup
    {
        [Parameter]
        public string? Titel { get; set; }

        [Parameter]
        public string? Message { get; set; }

        [Parameter]
        public string? TrueButton { get; set; }

        [Parameter]
        public string? FalseButton { get; set; }

        [Parameter]
        public bool IsVisible { get; set; }

        [Parameter]
        public EventCallback<bool> OnClose { get; set; }

        private async Task HandleClose(bool result)
        {
            await OnClose.InvokeAsync(result);
        }
    }
}
