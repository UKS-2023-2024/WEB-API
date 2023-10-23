using Library.Auth.Interfaces;
using Library.Auth.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Library;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}