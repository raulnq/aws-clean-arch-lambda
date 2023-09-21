using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.Shared.Domain;

namespace MyECommerceApp.Shared.Infrastructure.EntityFramework;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration["DbConnectionString"])
            .ConfigureWarnings(w => w.Ignore(SqlServerEventId.SavepointsDisabledBecauseOfMARS)));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

        services.AddScoped<TransactionBehavior>();

        return services;
    }
}