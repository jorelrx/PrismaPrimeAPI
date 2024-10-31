namespace PrismaPrimeInvest.Domain.Entities.Invest;

public class FundPayment : BaseEntity
{
    public DateTime PaymentDate { get; set; }
    public double Price { get; set; }
    public double Dividend { get; set; }
    public double MinimumValue { get; set; }
    public double MaximumValue { get; set; }
    public DateTime MinimumValueDate { get; set; }
    public DateTime MaximumValueDate { get; set; }

    public Guid FundId { get; set; }
    public required Fund Fund { get; set; }
}
