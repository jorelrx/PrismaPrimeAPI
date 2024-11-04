using PrismaPrimeInvest.Domain.Entities.Relationships;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Relationships;
using PrismaPrimeInvest.Infra.Data.Contexts;

namespace PrismaPrimeInvest.Infra.Data.Repositories.Relationship;

public class WalletFundRepository(ApplicationDbContext context) : BaseRepository<WalletFund>(context), IWalletFundRepository
{
}