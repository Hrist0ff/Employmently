namespace employmently_be.Data.Models
{
    public class GetUserByIdDto
    {
        public string Id { get; set;}
        public string UserName { get; set;}
        public string Email     { get; set;}
        public string PhoneNumber { get; set;}
        public string? UniqueIdentifierCompany { get; set;}
        public string? Description { get; set;}
    }
}
