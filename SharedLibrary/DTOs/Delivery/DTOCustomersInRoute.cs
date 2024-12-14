namespace SharedLibrary.DTOs.Delivery
{
    public class DTOCustomersInRoute
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public DTOCustomerSequence[]? CustomerSequence { get; set; }
    }

    public class DTOCustomerSequence
    {
        public long Id { get; init; }

        public int Position { get; set; }

        public string? Name { get; set; }
    }
}
