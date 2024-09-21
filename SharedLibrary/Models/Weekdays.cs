using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class Weekdays
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public bool Monday { get; set; } = false;
        
        [Required]
        public bool Tuesday { get; set; } = false;

        [Required]
        public bool Wednesday { get; set; } = false;

        [Required]
        public bool Thursday { get; set; } = false;

        [Required]
        public bool Friday { get; set; } = false;
        
        [Required]
        public bool Saturday { get; set; } = false;

        [Required]
        public bool Sunday { get; set; } = false;
    }
}
