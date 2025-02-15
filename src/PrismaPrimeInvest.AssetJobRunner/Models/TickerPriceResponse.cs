namespace PrismaPrimeInvest.AssetJobRunner.Models;

public class TickerPriceResponse
{
    public List<PriceDetail> Prices { get; set; } = [];

    public double MaxPrice => Prices.Max(p => p.Price);
    public double MinPrice => Prices.Min(p => p.Price);
    public double FirstPrice => Prices.FirstOrDefault()?.Price ?? 0;
    public double CurrentPrice => Prices.LastOrDefault()?.Price ?? 0;
    public DateTime CurrentDate => Prices.LastOrDefault()?.DateConterted ?? DateTime.Now;
}
