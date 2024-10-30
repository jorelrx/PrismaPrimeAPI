namespace PrismaPrimeInvest.Domain.Entities.User
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Document { get; set; }
    }
}