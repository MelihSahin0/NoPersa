using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Website.Client.Enums;

namespace Website.Client.Components.Base
{
    public partial class MonthPicker
    {
        [Parameter]
        public required string Id { get; set; }

        [Parameter] 
        public required int Year { get; set; }

        [Parameter]
        public EventCallback<int> YearChanged { get; set; }

        [Parameter]
        public Expression<Func<int>>? YearExpression { get; set; }

        [Parameter]
        public required Months Month { get; set; }

        [Parameter]
        public EventCallback<Months> MonthChanged { get; set; }

        [Parameter]
        public Expression<Func<Months>>? MonthExpression { get; set; }

        [Parameter]
        public EventCallback OnMonthYearSelected { get; set; }

        private bool isDropdownOpen = false;
        private static readonly string[] months = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"];

        private async Task ToggleDropdown()
        {
            if (isDropdownOpen)
            {
                await YearChanged.InvokeAsync(Year);
                await MonthChanged.InvokeAsync(Month);
                await OnMonthYearSelected.InvokeAsync();
            }

            isDropdownOpen = !isDropdownOpen;
        }

        private async Task SelectMonth(int monthIndex)
        {
            Month = (Months)monthIndex;
            isDropdownOpen = false;
            await YearChanged.InvokeAsync(Year);
            await MonthChanged.InvokeAsync(Month);
            await OnMonthYearSelected.InvokeAsync();
        }

        private void ChangeYear(int change)
        {
            Year += change;
        }
    }
}
