namespace SharedLibrary.DTOs.Delivery
{
    public class DTORouteSummary
    {
        public long Id { get; set; }

        public int? Position { get; set; }

        public string? Name { get; set; }

        public bool IsDrivable { get; set; }

        public int? NumberOfCustomers { get; set; }
    }
}