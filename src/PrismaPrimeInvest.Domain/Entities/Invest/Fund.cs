using PrismaPrimeInvest.Domain.Entities.Relationships;
using PrismaPrimeInvest.Domain.Enums;

namespace PrismaPrimeInvest.Domain.Entities.Invest;

public class Fund : BaseEntity
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required double Price { get; set; }
    public required double MaxPrice { get; set; }
    public required double MinPrice { get; set; }
    public int BestBuyDay { get; set; }
    public double BestBuyDayPrice { get; set; }
    public FundTypeEnum Type { get; set; }

    public ICollection<FundDailyPrice>? DailyPrices { get; set;}
    public ICollection<FundPayment>? Payments { get; set; }

    public ICollection<WalletFund> WalletFunds { get; set; } = [];
}
