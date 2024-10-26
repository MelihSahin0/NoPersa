using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using Website.Client.Enums;

namespace Website.Client.Models
{
    public class MonthOfTheYear
    {
        [Required]
        public required Months Month { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Year { get; set; }
    }
}
