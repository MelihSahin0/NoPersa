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

        public override bool Equals(object? obj)
        {
            if (obj is not Weekdays)
            {
                return false;
            }

            Weekdays weekday = (Weekdays)obj;
            if ((Monday == weekday.Monday) && (Tuesday == weekday.Tuesday) && (Wednesday == weekday.Wednesday) &&
               (Thursday == weekday.Thursday) && (Friday == weekday.Friday) && (Saturday == weekday.Saturday) && 
               (Sunday == weekday.Sunday))
            {
                return true;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
