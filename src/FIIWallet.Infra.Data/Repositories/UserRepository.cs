using FIIWallet.Domain.Entities;
using FIIWallet.Domain.Repositories;
using FIIWallet.Infra.Data.Contexts;

namespace FIIWallet.Infra.Data.Repositories;
public class UserRepository(ApplicationDbContext context) : BaseRepository<User>(context), IUserRepository {}