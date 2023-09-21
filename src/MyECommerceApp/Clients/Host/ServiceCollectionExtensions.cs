using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.Clients.Application;

namespace MyECommerceApp.Clients.Host;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClients(this IServiceCollection services)
    {
        services.AddTransient<RegisterClient.CommandHandler>();

        return services;
    }
}
