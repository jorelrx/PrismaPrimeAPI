using PrismaPrimeInvest.Domain.Entities.User;

namespace PrismaPrimeInvest.Domain.Entities.Relationships;

public class WalletUser : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid WalletId { get; set; }

    public required Wallet Wallet { get; set; }
    public required User.User User { get; set; }
}
