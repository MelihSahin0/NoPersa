
namespace SharedLibrary.DTOs
{
    public class DTOSequenceDetails
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public DTOCustomerSequence[]? CustomersRoute { get; set; }
    }
}
