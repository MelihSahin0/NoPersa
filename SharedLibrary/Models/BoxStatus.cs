using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SharedLibrary.Models
{
    public class BoxStatus
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public required int NumberOfBoxesPreviousDay { get; set; }

        [Required]
        public required int DeliveredBoxes {  get; set; }

        [Required]
        public required int ReceivedBoxes { get; set; }

        [Required]
        public required int NumberOfBoxesCurrentDay { get; set; }

        [Required]
        [ForeignKey("CustomerId")]
        public required long CustomerId { get; set; }

        public Customer? Customer { get; set; }
    }
}
