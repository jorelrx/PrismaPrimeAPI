using Microsoft.Extensions.DependencyInjection;

using PrismaPrimeInvest.Application.Interfaces.Services.Invest;
using PrismaPrimeInvest.Application.Services.Invest;

using PrismaPrimeInvest.Application.Interfaces.Services.UserInterfaces;
using PrismaPrimeInvest.Application.Services.UserServices;

using PrismaPrimeInvest.Application.Interfaces.Services.Utilities;
using PrismaPrimeInvest.Application.Services.Utilities;

using PrismaPrimeInvest.Application.Interfaces.Services.Relationships;
using PrismaPrimeInvest.Application.Services.Relationships;
using Microsoft.AspNetCore.Http;

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
        services.AddScoped<IFundFavoriteService, FundFavoriteService>();
        services.AddScoped<IWalletUserService, WalletUserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}