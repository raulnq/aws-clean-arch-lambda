using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.Orders.Application;
using MyECommerceApp.Orders.Domain;

namespace MyECommerceApp.Orders.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrders(this IServiceCollection services)
    {
        services.AddTransient<PlaceOrder.Handler>();

        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
