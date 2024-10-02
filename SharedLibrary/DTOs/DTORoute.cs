
namespace SharedLibrary.DTOs
{
    public class DTORoute
    {
        public int Id { get; set; }

        public int? Position { get; set; }

        public string? Name { get; set; }

        public DTOCustomerRoute[]? CustomersRoute { get; set; }
    }
}
