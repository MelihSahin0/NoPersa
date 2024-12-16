namespace NoPersaService.DTOs.Article.Answer
{
    public class DTOSelectArticleWithPrice
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public double? Price { get; set; }

        public bool IsDefault { get; set; }
    }
}
