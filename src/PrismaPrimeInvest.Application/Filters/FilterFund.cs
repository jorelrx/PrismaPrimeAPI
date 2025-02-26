namespace PrismaPrimeInvest.Application.Filters;

public class FilterFund : FilterBase
{
    public string? Code { get; set; }
    public double? MinDividendYield { get; set; }
    public double? MaxDividendYield { get; set; }
    public double? MinNetAssetValue { get; set; }
    public double? MaxNetAssetValue { get; set; }
    public double? MinPvp { get; set; }
    public double? MaxPvp { get; set; }
}