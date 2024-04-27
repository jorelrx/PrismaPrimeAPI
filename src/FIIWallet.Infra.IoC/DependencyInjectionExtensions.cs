using FIIWallet.Infra.IoC.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace FIIWallet.Infra.IoC;
public static class DependencyInjectionExtensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddRepositories();
        services.AddServices();
    }
}
