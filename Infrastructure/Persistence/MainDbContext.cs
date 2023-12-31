﻿using System.Reflection;
using Domain.Auth;
using Domain.Branches;
using Domain.Milestones;
using Domain.Organizations;
using Domain.Repositories;
using Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Task = System.Threading.Tasks.Task;

namespace Infrastructure.Persistence;

public class MainDbContext: DbContext
{
    
    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Email> Emails { get; set; } = null!;
    public DbSet<SocialAccount> SocialAccounts { get; set; } = null!;
    public DbSet<Organization> Organizations { get; set; } = null!;
    public DbSet<OrganizationMember> OrganizationMembers { get; set; } = null!;
    public DbSet<Repository> Repositories { get; set; } = null!;
    public DbSet<RepositoryMember> RepositoryMembers { get; set; } = null!;
    public DbSet<OrganizationRole> OrganizationRoles { get; set; } = null!;
    public DbSet<OrganizationPermission> OrganizationPermissions { get; set; } = null!;
    public DbSet<OrganizationRolePermission> OrganizationRolePermissions { get; set; } = null!;
    public DbSet<OrganizationInvite> OrganizationInvites { get; set; } = null!;
    public DbSet<RepositoryInvite> RepositoryInvites { get; set; } = null!;
    public DbSet<Milestone> Milestones { get; set; } = null!;
    public DbSet<Branch> Branches { get; set; } = null!;
    public DbSet<Domain.Tasks.Task> Tasks { get; set; } = null!;
    public DbSet<Label> Labels { get; set; } = null!;
    public DbSet<Event> Events { get; set; } = null!;
    public DbSet<Issue> Issues { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

}