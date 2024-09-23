using System.ComponentModel.DataAnnotations;
using Website.Client.Enums;

namespace Website.Client.Models
{
    public class MonthOfTheYear
    {
        [Required]
        public required Months Month { get; set; }

        [Required]
        public required string Year { get; set; }
    }
}
