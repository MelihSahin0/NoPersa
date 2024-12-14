namespace SharedLibrary.DTOs.Delivery
{
    public class DTOCustomerDeliveryStatus
    {
        public long Id { get; init; }

        public int? Position { get; set; }

        public string? Name { get; set; }

        public bool? ToDeliver { get; set; }
    }
}
