﻿using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.ClientRequests.Application;

namespace MyECommerceApp.ClientRequests.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClientRequests(this IServiceCollection services)
    {
        services.AddTransient<RegisterClientRequest.Handler>();

        services.AddTransient<ApproveClientRequest.Handler>();

        services.AddTransient<ListClientRequests.Runner>();

        services.AddTransient<GetClientRequest.Runner>();

        services.AddTransient<AnyClientRequests.Runner>();

        return services;
    }
}
