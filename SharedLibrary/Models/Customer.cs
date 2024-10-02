using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(64)]
        public string? SerialNumber { get; set; }

        [MaxLength(64)]
        public string? Title { get; set; }
        
        [Required]
        [MaxLength(64)]
        public required string Name {  get; set; }

        [Required]
        [MaxLength(64)]
        public required string Address { get; set; }

        [Required]
        [MaxLength(64)]
        public required string Region { get; set; }

        [MaxLength(64)]
        public string? GeoLocation { get; set; }

        [MaxLength(64)]
        public string? ContactInformation { get; set; }

        [Required]
        public required int Article { get; set; }

        [Required]
        [DoubleType(min: 0)]
        public required double DefaultPrice { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int DefaultNumberOfBoxes { get; set; }

        public ICollection<MonthlyOverview> MonthlyOverviews { get; set; } = [];

        [IntType(min: 0)]
        public int? Position { get; set; }

        [ForeignKey("WorkdaysId")]
        public int WorkdaysId { get; set; }

        [Required]
        public required Weekdays Workdays { get; set; }

        [ForeignKey("HolidaysId")]
        public int HolidaysId { get; set; }

        [Required]
        public required Weekdays Holidays { get; set; }

        [ForeignKey("RouteId")]
        public int? RouteId { get; set; }

        public Route? Route { get; set; }
    }
}
