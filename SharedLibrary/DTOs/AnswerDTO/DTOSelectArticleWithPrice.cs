namespace SharedLibrary.DTOs.AnswerDTO
{
    public class DTOSelectArticleWithPrice
    {
        public long Id { get; set; }

        public string? Name { get; set; }

        public double? Price { get; set; }

        public bool IsDefault { get; set; }
    }
}
