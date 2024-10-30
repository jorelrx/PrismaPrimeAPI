namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs;

public class CreateFundDto
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Type { get; set; }
}