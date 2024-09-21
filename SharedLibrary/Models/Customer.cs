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
        
        [ForeignKey("WorkdaysId")]
        public int WorkdaysId { get; set; }

        [Required]
        public required Weekdays Workdays { get; set; }

        [ForeignKey("HolidaysId")]
        public int HolidaysId { get; set; }

        [Required]
        public required Weekdays Holidays { get; set; }
    }
}
