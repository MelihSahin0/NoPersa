
using NoPersaService.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public class StaticCustomersMenuPlan
    {
        public static List<CustomersMenuPlan> GetCustomersMenuPlan() =>
        [
            new() { CustomerId = 1, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 1)!, BoxContentId = 1, BoxContent = StaticBoxContents.GetBoxContents().FirstOrDefault(b => b.Id == 1)!, PortionSizeId = 1, PortionSize = StaticPortionSizes.GetPortionSizes().FirstOrDefault(p => p.Id == 1)! },
            new() { CustomerId = 1, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 1)!, BoxContentId = 2, BoxContent = StaticBoxContents.GetBoxContents().FirstOrDefault(b => b.Id == 2)!, PortionSizeId = 1, PortionSize = StaticPortionSizes.GetPortionSizes().FirstOrDefault(p => p.Id == 1)!},
            new() { CustomerId = 2, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 2)!, BoxContentId = 1, BoxContent = StaticBoxContents.GetBoxContents().FirstOrDefault(b => b.Id == 1)!, PortionSizeId = 2, PortionSize = StaticPortionSizes.GetPortionSizes().FirstOrDefault(p => p.Id == 2)!},
            new() { CustomerId = 2, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 2)!, BoxContentId = 2, BoxContent = StaticBoxContents.GetBoxContents().FirstOrDefault(b => b.Id == 2)!, PortionSizeId = 2, PortionSize = StaticPortionSizes.GetPortionSizes().FirstOrDefault(p => p.Id == 2)!},
        ];
    }
}
