
namespace SharedLibrary.Models
{
    public class CustomersMenuPlan
    {
        public int CustomerId { get; set; }

        public required Customer Customer { get; set; }

        public int BoxContentId { get; set; }

        public required BoxContent BoxContent { get; set; }

        public int PortionSizeId { get; set; }

        public required PortionSize PortionSize { get; set; }
    }
}
