using PrismaPrimeInvest.Domain.Entities.User;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.UserInterface;
using PrismaPrimeInvest.Infra.Data.Contexts;

namespace PrismaPrimeInvest.Infra.Data.Repositories.UserRepository;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository {}