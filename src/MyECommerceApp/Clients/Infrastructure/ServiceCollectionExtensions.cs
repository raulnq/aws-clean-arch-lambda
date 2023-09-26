using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.Clients.Application;
using MyECommerceApp.Clients.Infrastructure;

namespace MyECommerceApp.Clients.Host;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddTransient<RegisterClient.Handler>();

        services.AddTransient<GetClients.Runner>();

        return services;
    }
}
