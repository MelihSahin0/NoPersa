using Website.Client.Components.Default;

namespace Website.Client.Util
{
    public static class Misc
    {
        public static List<SelectInput> GetDefaultNumberOfBoxesSelection => [
            new SelectInput() { Id = 0, Value = "0"},
            new SelectInput() { Id = 1, Value = "1"},
            new SelectInput() { Id = 2, Value = "2"},
            new SelectInput() { Id = 3, Value = "3"},
            new SelectInput() { Id = 4, Value = "4"},
            new SelectInput() { Id = 5, Value = "5"},
            new SelectInput() { Id = 6, Value = "6"},
            new SelectInput() { Id = 7, Value = "7"},
            new SelectInput() { Id = 8, Value = "8"},
            new SelectInput() { Id = 9, Value = "9"},
            new SelectInput() { Id = 10, Value = "10"},
            ];
    }
}
