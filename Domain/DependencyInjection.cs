using Library.Auth.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Library;

public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        return services;
    }
}