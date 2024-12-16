using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class DeliverSummary
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Address { get; set; }

        [Required]
        public required double Latitude { get; set; }

        [Required]
        public required double Longitude { get; set; }

        public string? DeliveryWishes { get; set; }

        [Required]
        public required int NumberOfBoxes { get; set; }
    }
}
