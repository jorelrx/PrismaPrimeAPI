namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs.FundPayment;

public class CreateFundPaymentDto
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Type { get; set; }
}