namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;

public class UpdateFundDto
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Type { get; set; }
    public required double Price { get; set; }
    public required double MaxPrice { get; set; }
    public required double MinPrice { get; set; }
    public int BestBuyDay { get; set; }
    public double BestBuyDayPrice { get; set; }
}