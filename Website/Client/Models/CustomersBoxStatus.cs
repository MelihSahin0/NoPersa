using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class CustomersBoxStatus
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        [IntType(min: 0, max: 10)]
        public required int DeliveredBoxes { get; set; }

        [Required]
        [IntType(min: 0, max: 10)]
        public required int ReceivedBoxes { get; set; }
    }
}
