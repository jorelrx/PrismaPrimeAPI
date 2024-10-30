namespace PrismaPrimeInvest.Application.DTOs.UserDTOs;
public class CreateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Document { get; set; }
}