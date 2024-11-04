namespace PrismaPrimeInvest.Domain.Entities.Invest;

public class FundPayment : BaseEntity
{
    public DateTime PaymentDate { get; set; }
    public double Price { get; set; }
    public double Dividend { get; set; }
    public double MinimumPrice { get; set; }
    public double MaximumPrice { get; set; }
    public DateTime MinimumPriceDate { get; set; }
    public DateTime MaximumPriceDate { get; set; }

    public Guid FundId { get; set; }
    public required Fund Fund { get; set; }
}
