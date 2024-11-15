using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PrismaPrimeInvest.Application.DTOs.AuthDTOs;
using PrismaPrimeInvest.Application.Responses;
using PrismaPrimeInvest.Domain.Entities.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;

namespace PrismaPrimeInvest.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration configuration) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        User? user = await _userManager.FindByEmailAsync(model.Email) ?? throw new AuthenticationException(
            "Invalid credentials. User does not exist."
        );
            
        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (!result.Succeeded)
        {
            throw new AuthenticationException("Invalid credentials. Incorrect password.");
        }

        var token = GenerateJwtToken(user);

        ApiResponse<LoginResponse> response = new()
        {
            Id = Guid.NewGuid(),
            Status = System.Net.HttpStatusCode.OK,
            Response = new LoginResponse
            {
                Token = token,
                User = user
            },
            Message = "Login successful",
        };

        return Ok(response);
    }

    private string GenerateJwtToken(User user)
    {
        Claim[]? claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.FirstName),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        ];

        string? secretKey = _configuration["Jwt:SecretKey"] ?? throw new Exception("Secret key not found in configuration.");
        SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(secretKey));
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
