using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.ShoppingCart.Application;
using MyECommerceApp.ShoppingCart.Domain;

namespace MyECommerceApp.ShoppingCart.Infrastructure;

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
