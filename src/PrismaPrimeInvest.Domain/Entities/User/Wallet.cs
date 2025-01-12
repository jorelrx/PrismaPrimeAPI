using PrismaPrimeInvest.Domain.Entities.Relationships;

namespace PrismaPrimeInvest.Domain.Entities.User;

public class Wallet : BaseEntity
{
    public required string Name { get; set; }
    public required Guid CreatedByUserId { get; set; }
    public bool IsPublic { get; set; } = false;

    public User? CreatedByUser { get; set; }
    public ICollection<WalletUser> WalletUsers { get; set; } = [];
    public ICollection<WalletFund> WalletFunds { get; set; } = [];
}
