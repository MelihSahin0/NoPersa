namespace SharedLibrary.DTOs.Delivery
{
    public class DTOCustomersLocation
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? DeliveryWishes { get; set; }
    }
}
