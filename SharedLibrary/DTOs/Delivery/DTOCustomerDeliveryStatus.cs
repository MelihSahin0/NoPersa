namespace SharedLibrary.DTOs.Delivery
{
    public class DTOCustomerDeliveryStatus
    {
        public int Id { get; init; }

        public int? Position { get; set; }

        public string? Name { get; set; }

        public bool? ToDeliver { get; set; }
    }
}
