using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class BoxStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public required int NumberOfBoxesPreviousDay { get; set; }

        public int? DeliveredBoxes {  get; set; }

        public int? ReceivedBoxes { get; set; }

        [Required]
        public required int NumberOfBoxesCurrentDay { get; set; }

        public int CustomerId { get; set; }

        public required Customer Customer { get; set; }
    }
}
