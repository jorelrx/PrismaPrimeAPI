using Microsoft.Extensions.DependencyInjection;

using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Services.Invest;

using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Services.UserServices;

using PrismaPrimeInvest.Application.Interfaces.Services.Utilities;
using PrismaPrimeInvest.Application.Services.Utilities;

namespace PrismaPrimeInvest.Infra.IoC.DependencyInjection;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<HttpClient>();
        services.AddScoped<AssetHttpService>();

        services.AddScoped<IFundService, FundService>();
        services.AddScoped<IWalletService, WalletService>();
        services.AddScoped<IFundDailyPriceService, FundDailyPriceService>();
        services.AddScoped<IFundPaymentService, FundPaymentService>();
        services.AddScoped<IAssetReportDownloader, AssetReportDownloader>();
        services.AddScoped<IUserService, UserService>();
    }
}