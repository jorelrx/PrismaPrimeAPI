namespace PrismaPrimeInvest.AssetJobRunner.Models;

public class TickerPriceResponse
{
    public int CurrencyType { get; set; }
    public string Currency { get; set; }
    public string Symbol { get; set; }
    public List<PriceDetail> Prices { get; set; }

    public double MaxPrice => Prices?.Max(p => p.Price) ?? 0;
    public double MinPrice => Prices?.Min(p => p.Price) ?? 0;
    public double CurrentPrice => Prices?.LastOrDefault()?.Price ?? 0;
    public DateTime CurrentDate => Prices?.LastOrDefault()?.DateConterted ?? DateTime.Now;
}
