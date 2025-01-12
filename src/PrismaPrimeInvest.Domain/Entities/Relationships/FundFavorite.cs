using PrismaPrimeInvest.Domain.Entities.Invest;

namespace PrismaPrimeInvest.Domain.Entities.Relationships;

public class FundFavorite : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid FundId { get; set; }

    public required User.User User { get; set; }
    public required Fund Fund { get; set; }
}
