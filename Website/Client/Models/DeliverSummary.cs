namespace Website.Client.Models
{
    public class DeliverSummary
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Address { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string? DeliveryWishes { get; set; }
        public int NumberOfBoxes { get; set; }
    }
}
