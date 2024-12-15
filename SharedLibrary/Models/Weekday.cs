using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class Weekday
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required bool Monday { get; set; }
        
        [Required]
        public required bool Tuesday { get; set; }

        [Required]
        public required bool Wednesday { get; set; }

        [Required]
        public required bool Thursday { get; set; }

        [Required]
        public required bool Friday { get; set; }
        
        [Required]
        public required bool Saturday { get; set; }

        [Required]
        public required bool Sunday { get; set; }

        public override bool Equals(object? obj)
        {
           return obj is Weekday weekday &&
               Monday == weekday.Monday &&
               Tuesday == weekday.Tuesday &&
               Wednesday == weekday.Wednesday &&
               Thursday == weekday.Thursday &&
               Friday == weekday.Friday &&
               Saturday == weekday.Saturday &&
               Sunday == weekday.Sunday;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
