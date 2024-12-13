using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public static class StaticBoxStatus
    {
        public static List<BoxStatus> GetBoxStatuses() =>
        [
            new() { Id = 1, NumberOfBoxesPreviousDay = 1, DeliveredBoxes = 1, ReceivedBoxes = 1, NumberOfBoxesCurrentDay = 1, CustomerId = 1, Customer = StaticCustomers.GetCustomers().First(c => c.Id == 1) },
            new() { Id = 2, NumberOfBoxesPreviousDay = 2, DeliveredBoxes = 2, ReceivedBoxes = 2, NumberOfBoxesCurrentDay = 2, CustomerId = 2, Customer = StaticCustomers.GetCustomers().First(c => c.Id == 2) }
        ];
    }
}
