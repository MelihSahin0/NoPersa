using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class BoxStatusOverview
    {
        [Required]
        public required int Id { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int NumberOfBoxesPreviousDay { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int DeliveredBoxes { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int ReceivedBoxes { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int NumberOfBoxesCurrentDay { get; set; }

        [Required]
        public required string CustomersName { get; set; }

        [Required]
        public required string RouteName { get; set; }
    }
}
