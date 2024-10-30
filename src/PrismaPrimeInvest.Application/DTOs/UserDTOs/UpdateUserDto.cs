namespace PrismaPrimeInvest.Application.DTOs.UserDTOs;
public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Document { get; set; }
}