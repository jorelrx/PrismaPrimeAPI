namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;

public class CreateFundDailyPriceDto
{
    public DateTime Date { get; set; }
    public double Price { get; set; }
    public double MaxPrice { get; set; }
    public double MinPrice { get; set; }
    public Guid FundId { get; set; }
}