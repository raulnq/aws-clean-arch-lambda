using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.Orders.Application;
using MyECommerceApp.Orders.Domain;
using MyECommerceApp.Orders.Infrastructure;

namespace MyECommerceApp.Orders.Host;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrders(this IServiceCollection services)
    {
        services.AddTransient<PlaceOrder.Handler>();

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
