using PrismaPrimeInvest.Domain.Entities.User;

namespace PrismaPrimeInvest.Domain.Interfaces.Repositories.UserInterface;

public interface IWalletRepository : IBaseRepository<Wallet> 
{
    Task<Wallet?> GetWalletByUserId(Guid userId);
}