using PrismaPrimeInvest.Domain.Interfaces.Repositories;
using PrismaPrimeInvest.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace PrismaPrimeInvest.Infra.IoC.DependencyInjection;
public static class RepositoryExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}