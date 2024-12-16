
using NoPersaService.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public class StaticDeliveryLocation
    {
        public static List<DeliveryLocation> GetDeliveryLocations() =>
        [
            new() { Id = 1, Address = "Address 1", Region = "Region 1", Latitude = 1, Longitude = 1, CustomerId = 1, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 1)!},
            new() { Id = 2, Address = "Address 2", Region = "Region 2", Latitude = 2, Longitude = 2, CustomerId = 2, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 2)!},
        ];
    }
}
