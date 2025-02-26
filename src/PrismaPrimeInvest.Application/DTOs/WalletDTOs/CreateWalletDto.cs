using PrismaPrimeInvest.Domain.Enums;

namespace PrismaPrimeInvest.Application.DTOs.WalletDTOs;

public class CreateWalletDto
{
    public string? Name { get; set; }
    public WalletTypeEnum? WalletType { get; set; }
    public bool? IsPublic { get; set; }
}