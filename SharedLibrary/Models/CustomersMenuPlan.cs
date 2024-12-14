
namespace SharedLibrary.Models
{
    public class CustomersMenuPlan
    {
        public long CustomerId { get; set; }

        public required Customer Customer { get; set; }

        public long BoxContentId { get; set; }

        public required BoxContent BoxContent { get; set; }

        public long PortionSizeId { get; set; }

        public required PortionSize PortionSize { get; set; }
    }
}
