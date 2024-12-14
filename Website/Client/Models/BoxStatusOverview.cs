using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class BoxStatusOverview
    {
        [Required]
        public required long Id { get; set; }

        [Required]
        [LongType(min: 0, max: 10)]
        public required long NumberOfBoxesPreviousDay { get; set; }

        [Required]
        [LongType(min: 0, max: 10)]
        public required long DeliveredBoxes { get; set; }

        [Required]
        [LongType(min: 0, max: 10)]
        public required long ReceivedBoxes { get; set; }

        [Required]
        [LongType(min: 0, max: 10)]
        public required long NumberOfBoxesCurrentDay { get; set; }

        [Required]
        public required string CustomersName { get; set; }

        [Required]
        public required string RouteName { get; set; }
    }
}
