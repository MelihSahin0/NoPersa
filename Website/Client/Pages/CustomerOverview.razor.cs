using Microsoft.AspNetCore.Components;
using Website.Client.FormModels;

namespace Website.Client.Pages
{
    public partial class CustomerOverview
    {
        [SupplyParameterFromForm]
        private Customer? Customer {  get; set; }

        protected override void OnInitialized() => Customer ??= new() { Name = string.Empty};

        private void Submit()
        {

        }
    }
}
