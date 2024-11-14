using Microsoft.AspNetCore.Identity;
using PrismaPrimeInvest.Domain.Interfaces.Entities;

namespace PrismaPrimeInvest.Domain.Entities.User;

public class Role : IdentityRole<Guid>, IBaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public Role() : base() { }

    public Role(string roleName) : base(roleName) { }
}
