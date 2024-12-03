using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class Maintenance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required string Type { get; set; }

        [Required]
        public required DateTime Date { get; set; }
    }
}
