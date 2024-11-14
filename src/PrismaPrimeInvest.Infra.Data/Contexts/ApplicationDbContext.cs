using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PrismaPrimeInvest.Domain.Entities.User;

namespace PrismaPrimeInvest.Infra.Data.Contexts;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<
    User, 
    Role, 
    Guid,
    IdentityUserClaim<Guid>, 
    IdentityUserRole<Guid>, 
    IdentityUserLogin<Guid>, 
    IdentityRoleClaim<Guid>, 
    IdentityUserToken<Guid>
>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}