using System.ComponentModel.DataAnnotations;

namespace Website.Client.FormModels
{
    public class Customer
    {
        [Required(ErrorMessage = "Name is required.")]
        public required string? Name { get; set; }

        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; } = string.Empty;
    }
}
