namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;

public class FundPaymentDto : BaseDto
{
    public required new Guid Id { get; set; }
    public DateTime PaymentDate { get; set; }
    public double Price { get; set; }
    public double Dividend { get; set; }
    public double MinimumPrice { get; set; }
    public double MaximumPrice { get; set; }
    public DateTime MinimumPriceDate { get; set; }
    public DateTime MaximumPriceDate { get; set; }

    public Guid FundId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}