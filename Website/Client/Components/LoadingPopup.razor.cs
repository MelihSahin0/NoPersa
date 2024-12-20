using Microsoft.AspNetCore.Components;

namespace Website.Client.Components
{
    public partial class LoadingPopup
    {
        [Parameter]
        public required bool IsVisible { get; set; }

        [Parameter]
        public string Description { get; set; } = string.Empty;

    }
}
