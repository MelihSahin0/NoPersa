using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class Holiday
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required string Country { get; set; }

        [Required]
        public required int Year { get; set; }

        [Required]
        public required int Month { get; set; }

        [Required]
        public required int Day { get; set; }
    }

}