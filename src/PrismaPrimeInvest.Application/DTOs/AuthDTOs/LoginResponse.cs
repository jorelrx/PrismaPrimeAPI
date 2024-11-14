using PrismaPrimeInvest.Domain.Entities.User;

namespace PrismaPrimeInvest.Application.DTOs.AuthDTOs;

public class LoginResponse
{
    public string? Token { get; set; }
    public User? User { get; set; }
}