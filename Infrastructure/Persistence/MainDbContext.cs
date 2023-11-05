using System.Reflection;
using Domain.Auth;
using Domain.Organizations;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Email>()
            .HasKey(e => e.Value);
        
        modelBuilder.Entity<User>()
            .HasMany(u => u.SecondaryEmails)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<User>()
            .HasIndex(u => u.PrimaryEmail)
            .IsUnique();
        
        modelBuilder.Entity<Organization>()
            .HasMany(o => o.Members)
            .WithOne(m => m.Organization)
            .HasForeignKey(o => o.OrganizationId);

        modelBuilder.Entity<Organization>()
            .HasMany(o => o.PendingMembers)
            .WithMany(u => u.PendingOrganizations);

        modelBuilder.Entity<Repository>()
            .HasMany(o => o.Members)
            .WithOne(m => m.Repository)
            .HasForeignKey(o => o.RepositoryId);

        modelBuilder.Entity<Repository>()
            .HasMany(o => o.PendingMembers)
            .WithMany(u => u.PendingRepositories);

        modelBuilder.Entity<OrganizationRole>()
            .HasKey(r => r.Name);
        modelBuilder.Entity<OrganizationPermission>()
            .HasKey(p => p.Value);
        modelBuilder.Entity<OrganizationRolePermission>()
            .HasKey(o =>
            new {
                o.RoleName, o.PermissionName
            });
        modelBuilder.Entity<OrganizationRole>()
            .HasMany(o => o.Permissions)
            .WithOne(p => p.Role)
            .HasForeignKey(p => p.RoleName);

        modelBuilder.Entity<OrganizationPermission>()
            .HasMany(o => o.Roles)
            .WithOne(o => o.Permission)
            .HasForeignKey(o => o.PermissionName);
        
        modelBuilder.Entity<OrganizationMember>()
            .HasOne(mem => mem.Role)
            .WithMany(r => r.Members);


        //Seeding
        //Permissions
        modelBuilder.Entity<OrganizationPermission>()
            .HasData(GetSeedPermissions());
        //Roles
        modelBuilder.Entity<OrganizationRole>()
            .HasData(OrganizationRole.Owner(), OrganizationRole.Member());
        //Permission-Role connection
        modelBuilder.Entity<OrganizationRolePermission>()
            .HasData(GetSeedPermissionRole());
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }


    private List<OrganizationRolePermission> GetSeedPermissionRole()
        => new()
        {
            new("OWNER", "owner"),
            new("OWNER", "admin"),
            new("OWNER", "manager"),
            new("OWNER", "read_only"),
            new("MEMBER", "manager"),
            new("MEMBER", "read_only"),
        };
    private List<OrganizationPermission> GetSeedPermissions()
         => new() { new("owner"), new("admin"), new("manager"), new("read_only") };

}