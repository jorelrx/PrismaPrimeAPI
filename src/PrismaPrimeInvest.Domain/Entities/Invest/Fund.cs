using PrismaPrimeInvest.Domain.Enums;

namespace PrismaPrimeInvest.Domain.Entities.Invest;

public class Fund : BaseEntity
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public FundTypeEnum Type { get; set; }

    public ICollection<FundBestDay>? FundBestDays { get; set; }
    public ICollection<FundDailyValue>? FundDailyValue { get; set;}
    public ICollection<FundPayment>? FundPayments { get; set; }
}
