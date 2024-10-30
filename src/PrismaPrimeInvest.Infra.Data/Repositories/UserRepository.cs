using PrismaPrimeInvest.Domain.Entities;
using PrismaPrimeInvest.Domain.Interfaces.Repositories;
using PrismaPrimeInvest.Infra.Data.Contexts;

namespace PrismaPrimeInvest.Infra.Data.Repositories;

public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository {}