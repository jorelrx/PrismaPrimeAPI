namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;

public class FundDailyPriceDto : BaseDto
{
    public required new Guid Id { get; set; }
    public DateTime Date { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Type { get; set; }
    public required double OpenPrice { get; set; }
    public required double ClosePrice { get; set; }
    public required double MaxPrice { get; set; }
    public required double MinPrice { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}