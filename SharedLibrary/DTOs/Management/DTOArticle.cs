
namespace SharedLibrary.DTOs.Management
{
    public class DTOArticle
    {
        public int Id { get; set; }

        public int Position { get; set; }

        public string? Name { get; set; }

        public string? Price { get; set; }

        public string? NewName { get; set; }

        public string? NewPrice { get; set; }

        public int? NumberOfCustomers { get; set; }
    }
}
