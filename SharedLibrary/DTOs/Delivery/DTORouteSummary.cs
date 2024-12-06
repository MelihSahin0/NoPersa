namespace SharedLibrary.DTOs.Delivery
{
    public class DTORouteSummary
    {
        public int Id { get; set; }

        public int? Position { get; set; }

        public string? Name { get; set; }

        public bool IsDrivable { get; set; }

        public int? NumberOfCustomers { get; set; }
    }
}