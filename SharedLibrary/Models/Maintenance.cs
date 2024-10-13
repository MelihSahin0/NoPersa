using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class Maintenance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime NextDailyDeliverySave { get; set; }
    }
}
