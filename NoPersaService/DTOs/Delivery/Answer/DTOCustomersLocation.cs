namespace NoPersaService.DTOs.Delivery.Answer
{
    public class DTOCustomersLocation
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? DeliveryWishes { get; set; }
        public int NumberOfBoxes { get; set; }
    }
}
