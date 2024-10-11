using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class Holiday
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Country { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Year { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Month { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Day { get; set; }
    }

}