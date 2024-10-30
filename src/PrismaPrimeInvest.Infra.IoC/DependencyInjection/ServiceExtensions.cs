using Microsoft.Extensions.DependencyInjection;
using PrismaPrimeInvest.Application.Interfaces.Services;
using PrismaPrimeInvest.Application.Services;

namespace PrismaPrimeInvest.Infra.IoC.DependencyInjection;
public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}