using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Domain.Entities.User;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.UserInterface;
using PrismaPrimeInvest.Infra.Data.Contexts;

namespace PrismaPrimeInvest.Infra.Data.Repositories.UserRepository;

public class WalletRepository(ApplicationDbContext context) : BaseRepository<Wallet>(context), IWalletRepository
{

    public async Task<Wallet?> GetWalletByUserId(Guid userId)
    {
        try
        {
            Wallet? wallet = await _entity.FirstOrDefaultAsync();
            return wallet;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}