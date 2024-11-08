namespace SharedLibrary.DTOs.Delivery
{
    public class DTOCustomersInRoute
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DTOCustomerSequence[]? CustomerSequence { get; set; }
    }

    public class DTOCustomerSequence
    {
        public int Id { get; init; }

        public int Position { get; set; }

        public string? Name { get; set; }
    }
}
