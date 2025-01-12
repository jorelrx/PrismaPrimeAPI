namespace PrismaPrimeInvest.Application.DTOs.ResponsesDTOs;

public class DailyPriceResponse
{
    public int CurrencyType { get; set; }
    public string Currency { get; set; }
    public string Symbol { get; set; }
    public List<PriceDetail> Prices { get; set; } = new();
}
