using Microsoft.AspNetCore.Identity;
using PrismaPrimeInvest.Domain.Interfaces.Entities;

namespace PrismaPrimeInvest.Domain.Entities.User;

public class UserClaim : IdentityUserClaim<Guid>, IBaseEntity
{
    public new Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
