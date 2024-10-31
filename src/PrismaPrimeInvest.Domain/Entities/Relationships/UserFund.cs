using PrismaPrimeInvest.Domain.Entities.Invest;

namespace PrismaPrimeInvest.Domain.Entities.Relationships;

public class UserFund : BaseEntity
{
    public Guid FundId { get; set; }
    public Guid UserId { get; set; }
    public int Quantity { get; set; }

    public required User.User User { get; set; }
    public required Fund Fund { get; set; }
}
