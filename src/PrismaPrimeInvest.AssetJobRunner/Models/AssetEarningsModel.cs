namespace PrismaPrimeInvest.AssetJobRunner.Models;

public class AssetEarningsModel
{
    public int Y { get; set; }
    public int M { get; set; }
    public int D { get; set; }
    public string Ed { get; set; }
    public string Pd { get; set; }
    public string Et { get; set; }
    public string Etd { get; set; }
    public decimal V { get; set; }
    public string Sv { get; set; }
    public string Sov { get; set; }
    public bool Adj { get; set; }
}