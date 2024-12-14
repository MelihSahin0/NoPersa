namespace SharedLibrary.Models
{
    public class CustomersLightDiet
    {
        public long CustomerId { get; set; }

        public required Customer Customer { get; set; }

        public long LightDietId { get; set; }

        public required LightDiet LightDiet { get; set; }
    }
}
