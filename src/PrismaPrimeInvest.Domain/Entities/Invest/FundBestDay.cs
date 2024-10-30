namespace PrismaPrimeInvest.Domain.Entities.Invest;

public class FundBestDay : BaseEntity
{
    public Guid FundId { get; set; }
    public required Fund Fund { get; set; }
    public DateTime Date { get; set; }
    public int BestDay { get; set; }
    public double Price { get; set; }
}
