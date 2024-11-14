namespace PrismaPrimeInvest.Application.DTOs.UserDTOs;
public class UserDto : BaseDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Document { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime UpdatedAt { get; set; }

    public string? Email { get; set; }
    public string? Password { get; set; }
}