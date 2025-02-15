using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;

namespace PrismaPrimeInvest.Application.Services.UserServices;

public class AuthService : IAuthService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid? GetAuthenticatedUserId()
    {
        var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        
        return userIdClaim != null ? Guid.Parse(userIdClaim.Value) : null;
    }
}
