using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.ShoppingCart.Application;
using MyECommerceApp.ShoppingCart.Domain;
using MyECommerceApp.ShoppingCart.Infrastructure;

namespace MyECommerceApp.ShoppingCart.Host;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AdShoppingCart(this IServiceCollection services)
    {
        services.AddTransient<AddProductToShoppingCart.Handler>();

        services.AddTransient<AnyShoppingCartItems.Runner>();

        services.AddTransient<ListShoppingCartItems.Runner>();

        services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

        return services;
    }
}
