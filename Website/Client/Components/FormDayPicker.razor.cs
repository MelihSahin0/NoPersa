using Microsoft.AspNetCore.Components;
using System.Linq.Expressions;
using Website.Client.Enums;

namespace Website.Client.Components
{
    public partial class FormDayPicker
    {
        private readonly string Id = Guid.NewGuid().ToString();

        [Parameter]
        public required string Label { get; init; }

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
        public required int Day { get; set; }

        [Parameter]
        public EventCallback<int> DayChanged { get; set; }

        [Parameter]
        public Expression<Func<int>>? DayExpression { get; set; }

        [Parameter]
        public EventCallback OnDayMonthYearSelected { get; set; }
    }
}
