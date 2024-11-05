using Microsoft.Extensions.DependencyInjection;

using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Services.Invest;

using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Services.UserServices;

namespace PrismaPrimeInvest.Infra.IoC.DependencyInjection;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IFundService, FundService>();
        services.AddScoped<IFundDailyPriceService, FundDailyPriceService>();
        services.AddScoped<IFundPaymentService, FundPaymentService>();
    }
}