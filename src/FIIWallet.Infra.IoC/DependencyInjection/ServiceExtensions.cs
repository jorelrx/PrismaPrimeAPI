using FIIWallet.Application.Interfaces;
using FIIWallet.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FIIWallet.Infra.IoC.DependencyInjection;
public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}