using PrismaPrimeInvest.Domain.Entities.Relationships;

namespace PrismaPrimeInvest.Domain.Entities.User
{
    public class User : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Document { get; set; }

        public required Wallet Wallet { get; set; }
    }
}