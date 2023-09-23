using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.Products.Application;
using MyECommerceApp.Products.Infrastructure;

namespace MyECommerceApp.Products.Host;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AdProducts(this IServiceCollection services)
    {
        services.AddTransient<RegisterProduct.Handler>();

        services.AddTransient<AnyProducts.Runner>();

        services.AddTransient<EnableProduct.Handler>();

        return services;
    }
}
