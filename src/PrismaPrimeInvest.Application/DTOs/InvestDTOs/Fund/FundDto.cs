namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;

public class FundDto : BaseDto
{
    public required new Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Cnpj { get; set; }
    public required string Code { get; set; }
    public required int QtyQuotasIssued { get; set; }
    public required double NetAssetValue { get; set; }
    public required double TotalShares { get; set; }
    public required double NetAssetValuePerShare { get; set; }
    public required double Price { get; set; }
    public required double MaxPrice { get; set; }
    public required double MinPrice { get; set; }
    public int BestBuyDay { get; set; }
    public double BestBuyDayPrice { get; set; }
    public required string Type { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}