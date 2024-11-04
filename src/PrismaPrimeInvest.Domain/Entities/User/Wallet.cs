using PrismaPrimeInvest.Domain.Entities.Relationships;

namespace PrismaPrimeInvest.Domain.Entities.User;

public class Wallet : BaseEntity
{
    public required Guid UserId { get; set; }
    public required User User { get; set; }
    public ICollection<WalletFund> WalletFunds { get; set; } = [];
}
