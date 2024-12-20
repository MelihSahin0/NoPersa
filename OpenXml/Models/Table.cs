namespace OpenXml.Models
{
    public class Table
    {
        public required string Name { get; set; }
        public required List<string> Headers { get; set; }
        public required List<List<object?>> Data { get; set; }
        public List<DropDownInformation> DropDownInformations { get; set; } = [];

        public static string Sanitize(string text)
        {
            char[] invalidChars = [':', '\\', '/', '?', '*', '[', ']', '\'', '\"'];
            string sanitized = new(text.Trim().Where(c => !invalidChars.Contains(c)).ToArray());
            sanitized = sanitized.Replace(" ", "_").Trim();

            if ("history".Equals(sanitized.ToLower()))
            {
                sanitized = string.Empty;
            }
            if (sanitized.Length > 31)
            {
                sanitized = sanitized[..31];
            }

            return sanitized;
        }

        public class DropDownInformation()
        {
            public required string Column { get; set; }
            public required int StartRow { get; set; }
            public int EndRow { get; set; } = 1048576;
            public required List<string> Data { get; set; }
            public bool AllowBlank { get; set; } = false;
        }
    }
}
