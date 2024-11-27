namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;

public class MonthlyInvestmentReport
{
    public required string Date { get; set; }
    public double TotalInvestmentWithoutDividends { get; set; }
    public double TotalInvestmentWithDividends { get; set; }
    public double FundPriceOnPurchaseDay { get; set; }
    public double SharesPurchasedThisMonth { get; set; }
    public double TotalSharesPurchased { get; set; }
    public double TotalAvailableForInvestment { get; set; }
    public double DividendsThisMonth { get; set; }
    public double TotalDividends { get; set; }
    public double PortfolioValueAtEndOfMonth { get; set; }
}