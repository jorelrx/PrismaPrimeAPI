namespace PrismaPrimeInvest.Application.DTOs.WalletDTOs;

public class WalletDto : BaseDto
{

    public required string Name { get; set; }
    public required Guid CreatedByUserId { get; set; }
    public bool IsPublic { get; set; } = false;
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}