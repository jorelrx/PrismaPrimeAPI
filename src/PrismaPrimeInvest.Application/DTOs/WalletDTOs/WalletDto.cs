using PrismaPrimeInvest.Domain.Enums;

namespace PrismaPrimeInvest.Application.DTOs.WalletDTOs;

public class WalletDto : BaseDto
{
    public required string Name { get; set; }
    public required string CreatedByUserName { get; set; }
    public required WalletTypeEnum WalletType { get; set; }
    public bool IsPublic { get; set; }
    public double TotalInvested { get; set; }
    public double TotalCurrentValue { get; set; }
}
