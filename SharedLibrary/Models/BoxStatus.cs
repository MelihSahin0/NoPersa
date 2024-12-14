using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public class BoxStatus
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [IntType(min: 0, max: 10)]
        public required int NumberOfBoxesPreviousDay { get; set; }

        [Required]
        [IntType(min: 0, max: 10)]
        public required int DeliveredBoxes {  get; set; }

        [Required]
        [IntType(min: 0, max: 10)]
        public required int ReceivedBoxes { get; set; }

        [Required]
        [IntType(min: 0, max: 10)]
        public required int NumberOfBoxesCurrentDay { get; set; }

        [ForeignKey("CustomerId")]
        public long CustomerId { get; set; }

        public required Customer Customer { get; set; }
    }
}
