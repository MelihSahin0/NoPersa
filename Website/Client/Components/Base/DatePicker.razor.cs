using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Website.Client.Util;

namespace Website.Client.Components.Base
{
    public partial class DatePicker
    {
        [Parameter]
        public required string Id { get; set; }

        [Parameter] 
        public required string Year { get; set; }

        [Parameter]
        public EventCallback<string> YearChanged { get; set; }

        [Parameter]
        public Expression<Func<string>>? YearExpression { get; set; }

        [Parameter]
        public required Months Month { get; set; }

        [Parameter]
        public EventCallback<Months> MonthChanged { get; set; }

        [Parameter]
        public Expression<Func<Months>>? MonthExpression { get; set; }

        private bool isDropdownOpen = false;
        private int selectedYear;
        private static readonly string[] months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        protected override void OnInitialized()
        {
            selectedYear = int.Parse(Year);
        }

        private void ToggleDropdown()
        {
            isDropdownOpen = !isDropdownOpen;
        }

        private async Task SelectMonth(int monthIndex)
        {
            Year = selectedYear.ToString();
            Month = (Months)monthIndex;
            isDropdownOpen = false;
            await YearChanged.InvokeAsync(Year);
            await MonthChanged.InvokeAsync(Month);
        }

        private void ChangeYear(int change)
        {
            selectedYear += change;
        }
    }
}
