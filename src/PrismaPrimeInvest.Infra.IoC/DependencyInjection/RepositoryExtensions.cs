using Microsoft.Extensions.DependencyInjection;

using PrismaPrimeInvest.Domain.Interfaces.Repositories.UserInterface;
using PrismaPrimeInvest.Infra.Data.Repositories.UserRepository;

using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;
using PrismaPrimeInvest.Infra.Data.Repositories.Invest;

namespace PrismaPrimeInvest.Infra.IoC.DependencyInjection;

public static class RepositoryExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFundRepository, FundRepository>();
    }
}