using PrismaPrimeInvest.Domain.Entities.Relationships;

namespace PrismaPrimeInvest.Domain.Entities.User
{
    public class User : BaseEntity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Document { get; set; }

    public ICollection<UserFund> UserFunds { get; set; } = [];
    }
}