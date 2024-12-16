namespace NoPersaService.DTOs.Delivery.Answer
{
    public class DTORouteOverview
    {
        public string? Id { get; set; }

        public int? Position { get; set; }

        public string? Name { get; set; }

        public DTOCustomerDeliveryStatus[]? CustomerDeliveryStatus { get; set; }
    }
}
