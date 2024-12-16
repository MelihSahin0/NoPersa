namespace NoPersaService.DTOs.Delivery.RA
{
    public class DTOCustomersInRoute
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public DTOCustomerSequence[]? CustomerSequence { get; set; }
    }

    public class DTOCustomerSequence
    {
        public string? Id { get; init; }

        public int Position { get; set; }

        public string? Name { get; set; }
    }
}
