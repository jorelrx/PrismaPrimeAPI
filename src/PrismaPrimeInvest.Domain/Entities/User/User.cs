using Microsoft.AspNetCore.Identity;
using PrismaPrimeInvest.Domain.Entities.Relationships;
using PrismaPrimeInvest.Domain.Interfaces.Entities;

namespace PrismaPrimeInvest.Domain.Entities.User;

public class User : IdentityUser<Guid>, IBaseEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Document { get; set; }

    public ICollection<WalletUser> WalletUsers { get; set; } = [];
    public ICollection<FundFavorite> FavoriteFunds { get; set; } = [];
}
