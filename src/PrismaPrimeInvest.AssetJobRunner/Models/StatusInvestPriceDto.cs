namespace PrismaPrimeInvest.AssetJobRunner.Models;

public class StatusInvestPriceDto
{
    public int CurrencyType { get; set; }
    public string Currency { get; set; }
    public string Symbol { get; set; }
    public List<PriceDataDto> Prices { get; set; } = new();
}
