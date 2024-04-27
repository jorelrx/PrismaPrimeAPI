using FIIWallet.Domain.Repositories;
using FIIWallet.Infra.Data.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FIIWallet.Infra.IoC.DependencyInjection;
public static class RepositoryExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
    }
}