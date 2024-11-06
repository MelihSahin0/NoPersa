using Microsoft.AspNetCore.Components;

namespace Website.Client.Components
{
    public partial class CustomerStatusDisplay
    {
        [Parameter]
        public string Text { get; set; } = "";

        /**
         * To see svg use bg-lightGreen and bg-lightRed
         * */
        [Parameter]
        public string BackgroundColor { get; set; } = "bg-white";

        [Parameter]
        public EventCallback OnClick { get; set; }
    }
}
