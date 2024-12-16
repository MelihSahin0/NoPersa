namespace NoPersaService.DTOs.Gastro.Answer
{
    public class DTOCustomersFood
    {
        public string? Name { get; set; }

        public int Position { get; set; }

        public int NumberOfBoxes { get; set; }

        public List<string>? LightDiets { get; set; }

        public List<DTOCustomersBoxContent>? BoxContents { get; set; }
    }
}
