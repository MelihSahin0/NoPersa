namespace SharedLibrary.DTOs.Gastro
{
    public class DTOCustomersFood
    {
        public string? Name { get; set; }

        public int Position { get; set; }

        public List<string>? LightDiets { get; set; }

        public List<DTOCustomersBoxContent>? BoxContents { get; set; }
    }
}
