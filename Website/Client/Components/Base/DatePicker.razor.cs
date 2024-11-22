using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Website.Client.Enums;

namespace Website.Client.Components.Base
{
    public partial class DatePicker
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
        public EventCallback OnMonthSelected { get; set; }

        [Parameter]
        public required int Day { get; set; }

        [Parameter]
        public EventCallback<int> DayChanged { get; set; }

        [Parameter]
        public Expression<Func<int>>? DayExpression { get; set; }

        [Parameter]
        public EventCallback OnDayMonthYearSelected { get; set; }

        private bool isDropdownOpen = false;

        private async Task ToggleDropdown()
        {
            if (isDropdownOpen)
            {
                await YearChanged.InvokeAsync(Year);
                await MonthChanged.InvokeAsync(Month);
                await DayChanged.InvokeAsync(Day);
                await OnDayMonthYearSelected.InvokeAsync();
            }

            isDropdownOpen = !isDropdownOpen;
        }

        private async Task SelectDay(int day)
        {
            Day = day;
            isDropdownOpen = false;
            await YearChanged.InvokeAsync(Year);
            await MonthChanged.InvokeAsync(Month);
            await DayChanged.InvokeAsync(Day);
            await OnDayMonthYearSelected.InvokeAsync();
        }

        private void ChangeYear(int change)
        {
            Year += change;
        }

        private void ChangeMonth(int change)
        {
            Month = (Months)(((12 + (int)Month + change - 1) % 12) + 1);
        }
    }
}
