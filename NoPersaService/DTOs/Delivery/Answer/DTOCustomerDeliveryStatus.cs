namespace NoPersaService.DTOs.Delivery.Answer
{
    public class DTOCustomerDeliveryStatus
    {
        public string? Id { get; init; }

        public int? Position { get; set; }

        public string? Name { get; set; }

        public bool? ToDeliver { get; set; }
    }
}
