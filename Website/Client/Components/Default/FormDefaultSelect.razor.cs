using Microsoft.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Components.Default
{
    public partial class FormDefaultSelect<T>
    {
        private readonly string Id = Guid.NewGuid().ToString();

        [Parameter]
        public required string Class { get; init; }

        [Parameter]
        public bool IsColumn { get; set; } = false;

        [Parameter]
        public required string Label { get; init; }

        [Parameter]
        public List<SelectInput<T>>? SelectInputs { get; set; }

        [Parameter]
        public required T Value { get; set; }

        [Parameter]
        public EventCallback<T> ValueChanged { get; set; }

        private List<SelectInput<T>>? previousSelectInputs;

        protected override async Task OnParametersSetAsync()
        {
            if (!ListComparer(previousSelectInputs, SelectInputs))
            {
                previousSelectInputs = SelectInputs?.Select(x => new SelectInput<T>() { Id = x.Id, Value = x.Value }).ToList();
                await ValueChanged.InvokeAsync(Value);
            }
        }

        private static bool ListComparer(List<SelectInput<T>>? oldList, List<SelectInput<T>>? newList)
        {
            if (oldList == null || newList == null) return false;
            if (oldList.Count != newList.Count) return false;

            return !oldList.Where((t, i) => !CompareValues(t.Id, newList[i].Id) || !CompareValues(t.Value, newList[i].Value)).Any();
        }

        private static bool CompareValues(object? value1, object? value2)
        {
            if (value1 == null && value2 == null) return true;
            if (value1 == null || value2 == null) return false;

            if (value1 is double d1 && value2 is double d2)
            {
                return d1.Equals(d2);
            }
            if (value1 is int i1 && value2 is int i2)
            {
                return i1.Equals(i2);
            }
            if (value1 is long l1 && value2 is long l2)
            {
                return l1.Equals(l2);
            }
            if (value1 is string s1 && value2 is string s2)
            {
                return s1.Equals(s2);
            }

            return value1.Equals(value2);
        }

        private async Task OnValueChanged(ChangeEventArgs e)
        {
            if (e.Value != null)
            {
                T? parsedValue = default;

                if (typeof(T) == typeof(double) && double.TryParse(e.Value.ToString(), out var doubleValue))
                {
                    parsedValue = (T)(object)doubleValue;
                }
                else if (typeof(T) == typeof(int) && int.TryParse(e.Value.ToString(), out var intValue))
                {
                    parsedValue = (T)(object)intValue;
                }
                else if (typeof(T) == typeof(long) && long.TryParse(e.Value.ToString(), out var longValue))
                {
                    parsedValue = (T)(object)longValue;
                }
                else if (typeof(T) == typeof(string))
                {
                    parsedValue = (T)e.Value;  
                }

                if (parsedValue != null)
                {
                    Value = parsedValue;
                    await ValueChanged.InvokeAsync(Value);
                }
            }
        }
    }

    public class SelectInput<T>
    {
        [Required]
        public required T Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public required string Value { get; set; }
    }
}
