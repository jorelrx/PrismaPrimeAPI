namespace PrismaPrimeInvest.Application.DTOs.AuthDTOs;

public class LoginRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }
}