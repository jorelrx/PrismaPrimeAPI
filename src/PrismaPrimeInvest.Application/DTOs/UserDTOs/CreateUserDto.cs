namespace PrismaPrimeInvest.Application.DTOs.UserDTOs;
public class CreateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public required string Document { get; set; }

    public required string Email { get; set; }
    public required string UserName { get; set; }
    public required string Password { get; set; }
}