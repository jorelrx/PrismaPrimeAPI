namespace PrismaPrimeInvest.Application.DTOs.InvestDTOs;

public class FundDto : BaseDto
{
    public required new Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Code { get; set; }
    public required string Type { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }
}