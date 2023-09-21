using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MyECommerceApp.ClientRequests.Application;
using MyECommerceApp.ClientRequests.Infrastructure;
using System;

namespace MyECommerceApp.ClientRequests.Host;

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
