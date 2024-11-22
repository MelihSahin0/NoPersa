using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Website.Client.Styles;

namespace Website.Client.Components
{
    public partial class FormTextDisplay
    {
        private readonly string Id = Guid.NewGuid().ToString();

        [Parameter]
        public string LabelOrientation { get; set; } = "top";

        [Parameter]
        public string LabelTextOrientation { get; set; } = "left";

        [Parameter]
        public string TextOrientation { get; set; } = "text-left";

        [Parameter]
        public string? Class { get; init; }

        [Parameter]
        public required string Label { get; init; }

        [Parameter]
        public string? Value { get; set; }
    }
}
