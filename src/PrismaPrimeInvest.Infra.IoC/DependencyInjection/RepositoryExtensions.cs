using Microsoft.Extensions.DependencyInjection;

using PrismaPrimeInvest.Domain.Interfaces.Repositories.UserInterface;
using PrismaPrimeInvest.Infra.Data.Repositories.UserRepository;

using PrismaPrimeInvest.Domain.Interfaces.Repositories.Invest;
using PrismaPrimeInvest.Infra.Data.Repositories.Invest;
using PrismaPrimeInvest.Domain.Interfaces.Repositories.Relationships;
using PrismaPrimeInvest.Infra.Data.Repositories.Relationship;

namespace PrismaPrimeInvest.Infra.IoC.DependencyInjection;

public static class RepositoryExtensions
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        services.AddScoped<IFundRepository, FundRepository>();
        services.AddScoped<IFundDailyPriceRepository, FundDailyPriceRepository>();
        services.AddScoped<IFundPaymentRepository, FundPaymentRepository>();
        services.AddScoped<IWalletFundRepository, WalletFundRepository>();
        services.AddScoped<IFundFavoriteRepository, FundFavoriteRepository>();
    }
}