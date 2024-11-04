namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundDailyPrice;

public class CreateFundDailyPriceDto
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Type { get; set; }
}