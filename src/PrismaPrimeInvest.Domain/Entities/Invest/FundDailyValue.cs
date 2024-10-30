namespace PrismaPrimeInvest.Domain.Entities.Invest;

public class FundDailyValue : BaseEntity
{
    public DateTime Date { get; set; }
    public double MinValue { get; set; }
    public double MaxValue { get; set; }

    public Guid FundId { get; set; }
    public required Fund Fund { get; set; }
}
