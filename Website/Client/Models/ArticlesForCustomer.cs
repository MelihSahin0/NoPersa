using System.ComponentModel.DataAnnotations;

namespace Website.Client.Models
{
    public class ArticlesForCustomer
    {
        [Required]
        public required long Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required int Price { get; set; }

        [Required]
        public required bool IsDefault { get; set; }
    }
}
