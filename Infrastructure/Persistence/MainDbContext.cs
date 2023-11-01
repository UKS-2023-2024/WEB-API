using System.Reflection;
using Domain.Auth;
using Domain.Organizations;
using Microsoft.EntityFrameworkCore;

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
        
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}