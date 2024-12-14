namespace SharedLibrary.DTOs.Delivery
{
    public class DTORouteOverview
    {
        public long Id { get; set; }

        public int? Position { get; set; }

        public string? Name { get; set; }

        public DTOCustomerDeliveryStatus[]? CustomerDeliveryStatus { get; set; }
    }
}
