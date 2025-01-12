using PrismaPrimeInvest.Domain.Entities.Invest;
using PrismaPrimeInvest.Domain.Entities.User;

namespace PrismaPrimeInvest.Domain.Entities.Relationships;

public class WalletFund : BaseEntity
{
    public DateTime PurchaseDate { get; set; }
    public double PurchasePrice { get; set; }
    public int Quantity { get; set; }

    public required Guid FundId { get; set; }
    public required Guid WalletId { get; set; }

    public Wallet? Wallet { get; set; }
    public Fund? Fund { get; set; }
}
