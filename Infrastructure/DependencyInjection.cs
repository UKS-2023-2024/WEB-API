using Domain.Auth.Interfaces;
using Domain.Branches.Interfaces;
using Domain.Milestones.Interfaces;
using Domain.Organizations.Interfaces;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Infrastructure.Auth.Repositories;
using Infrastructure.Auth.Services;
using Infrastructure.Branches;
using Infrastructure.Milestones;
using Infrastructure.Organizations.Repositories;
using Infrastructure.Repositories.Repositories;
using Infrastructure.Shared.Email;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IHashingService, HashingService>();
        services.AddScoped<ISocialAccountRepository, SocialAccountRepository>();
        services.AddScoped<IOrganizationRepository, OrganizationRepository>();
        services.AddScoped<IOrganizationMemberRepository, OrganizationMemberRepository>();
        services.AddScoped<IRepositoryRepository, RepositoryRepository>();
        services.AddScoped<IRepositoryMemberRepository, RepositoryMemberRepository>();
        services.AddScoped<IEmailService, GmailEmailService>();
        services.AddScoped<IOrganizationInviteRepository, OrganizationInviteRepository>();
        services.AddScoped<IOrganizationRoleRepository, OrganizationRoleRepository>();
        services.AddScoped<IRepositoryInviteRepository, RepositoryInviteRepository>();
        services.AddScoped<IMilestoneRepository, MilestoneRepository>();
        services.AddScoped<IBranchRepository, BranchRepository>();
        return services;
    }    
}