using PrismaPrimeInvest.Domain.Entities.Relationships;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Relationships;
using PrismaPrimeInvest.Infra.Data.Contexts;

namespace PrismaPrimeInvest.Infra.Data.Repositories.Relationship;

public class WalletUserRepository(ApplicationDbContext context) : BaseRepository<WalletUser>(context), IWalletUserRepository
{
}