namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.Fund;

public class CreateFundDto
{
    public string Cnpj { get; set; } = string.Empty;
    public string Ticker { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}