namespace PrismaPrimeInvest.Application.DTOs.WalletFundDTOs;

public class CreateWalletFundDto
{
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Type { get; set; }
}