namespace PrismaPrimeInvest.Application.DTOs.WalletDTOs;

public class WalletInvestmentAnalysisDto
{
    public DateTime Date { get; set; }
    public double InvestedInMonth { get; set; }
    public double TotalCurrentValue { get; set; }
    public double TotalGrossInvested { get; set; }
    public double TotalDividends { get; set; }
    public double MonthlyEarnings { get; set; }
}
