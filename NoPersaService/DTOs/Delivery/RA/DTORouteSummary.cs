namespace NoPersaService.DTOs.Delivery.RA
{
    public class DTORouteSummary
    {
        public string? Id { get; set; }

        public int? Position { get; set; }

        public string? Name { get; set; }

        public bool IsDrivable { get; set; }

        public int? NumberOfCustomers { get; set; }
    }
}