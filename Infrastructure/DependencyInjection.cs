using Domain.Auth.Interfaces;
using Infrastructure.Auth.Repositories;
using Infrastructure.Auth.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHashingService, HashingService>();
        return services;
    }    
}