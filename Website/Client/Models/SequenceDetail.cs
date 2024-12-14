using SharedLibrary.Validations;
using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class SequenceDetail
    {
        [Required]
        public required long Id { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        [Required]
        public required List<CustomerSequence> CustomerSequence { get; set; }
    }

    public class CustomerSequence
    {
        [Required]
        public required long Id { get; set; }

        [Required]
        [IntType(min: 0)]
        public required int Position { get; set; }

        [Required]
        [StringLength(64, ErrorMessage = "Maximum allowed characters are 64.")]
        public required string Name { get; set; }

        public bool IsDragOver { get; set; }
    }
}
