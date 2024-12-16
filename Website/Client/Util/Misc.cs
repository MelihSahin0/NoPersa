using Website.Client.Components.Default;

namespace Website.Client.Util
{
    public static class Misc
    {
        public static List<SelectInput<int>> GetDefaultNumberOfBoxesSelection => [
            new SelectInput<int>() { Id = 0, Value = "0"},
            new SelectInput<int>() { Id = 1, Value = "1"},
            new SelectInput<int>() { Id = 2, Value = "2"},
            new SelectInput<int>() { Id = 3, Value = "3"},
            new SelectInput<int>() { Id = 4, Value = "4"},
            new SelectInput<int>() { Id = 5, Value = "5"},
            new SelectInput<int>() { Id = 6, Value = "6"},
            new SelectInput<int>() { Id = 7, Value = "7"},
            new SelectInput<int>() { Id = 8, Value = "8"},
            new SelectInput<int>() { Id = 9, Value = "9"},
            new SelectInput<int>() { Id = 10, Value = "10"},
            ];
    }
}
