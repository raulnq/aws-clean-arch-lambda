using Amazon.Lambda.Annotations;
using Amazon.Lambda.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.ClientRequests.Host;
using MyECommerceApp.Clients.Host;
using MyECommerceApp.Orders.Host;
using MyECommerceApp.Products.Host;
using MyECommerceApp.Shared.Infrastructure.EntityFramework;
using MyECommerceApp.Shared.Infrastructure.Messaging;
using MyECommerceApp.Shared.Infrastructure.SqlKata;
using MyECommerceApp.ShoppingCart.Host;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]
namespace MyECommerceApp;

[LambdaStartup]
public class Startup
{
    private readonly IConfigurationRoot Configuration;

    public Startup()
    {
        Configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddEntityFramework(Configuration);
        services.AddSqlKata(Configuration);
        services.AddMessaging(Configuration);
        services.AddClientRequests();
        services.AddClients();
        services.AdProducts();
        services.AdShoppingCart();
        services.AddOrders();
    }
}
