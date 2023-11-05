using Domain.Organizations.Interfaces;
using Domain.Organizations.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IPermissionService, PermissionService>();
        return services;
    }
}