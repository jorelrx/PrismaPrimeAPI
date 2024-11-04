namespace PrismaPrimeInvest.Application.DTOs.WalletDTOs;
public class UpdateWalletDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Document { get; set; }
}