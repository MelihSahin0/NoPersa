using System.ComponentModel.DataAnnotations;

namespace SharedLibrary.Models
{
    public class CustomersMenuPlan
    {
        [Required]
        public required long CustomerId { get; set; }

        public Customer? Customer { get; set; }

        [Required]
        public required long BoxContentId { get; set; }

        public BoxContent? BoxContent { get; set; }

        [Required]
        public required long PortionSizeId { get; set; }

        public PortionSize? PortionSize { get; set; }
    }
}
