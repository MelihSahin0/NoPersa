namespace NoPersaService.DTOs.Box.RA
{
    public class DTOBoxStatus
    {
        public string? Id { get; set; }

        public int NumberOfBoxesPreviousDay { get; set; }

        public int DeliveredBoxes { get; set; }

        public int ReceivedBoxes { get; set; }

        public int NumberOfBoxesCurrentDay { get; set; }

        public string? CustomersName { get; set; }

        public string? RouteName { get; set; }
    }
}
