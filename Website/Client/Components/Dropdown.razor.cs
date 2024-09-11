using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace Website.Client.Components
{
    public partial class Dropdown
    {
        [Parameter]
        public required string Title { get; init; }

        [Parameter]
        public required ReadOnlyCollection<string> Items { get; init; }

        [Parameter]
        public required ReadOnlyCollection<string> ItemsHref { get; init; }

        private bool dropdownIsVisible = false;

        private void ChangeDropdownVisibility()
        {
            dropdownIsVisible = !dropdownIsVisible;
        }
    
        private async Task CloseDropdown()
        {
            await Task.Delay(100);
            dropdownIsVisible = false;
        }
    } 
}
