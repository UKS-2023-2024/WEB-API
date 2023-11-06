using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Organizations;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace Tests.Integration.Setup;

public class TestDatabaseFactory : WebApplicationFactory<Program>
{
    public MainDbContext dbContext;
    public IConfiguration _configuration;
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        
        builder.ConfigureAppConfiguration((context, conf) =>
        {
            // expand default config with settings designed for Integration Tests
            conf.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"));
            conf.AddEnvironmentVariables();

            // here we can "compile" the settings. Api.Setup will do the same, it doesn't matter.
            _configuration = conf.Build();
        });
        builder.ConfigureServices(services =>
        {
            using var scope = BuildServiceProvider(services).CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<MainDbContext>();
            dbContext = db;
            InitializeDatabase(db);
        });
    }

    private ServiceProvider BuildServiceProvider(IServiceCollection services)
    {
        var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MainDbContext>));
        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }

        var connectionString = _configuration["TestConnectionString"];
        services.AddDbContext<MainDbContext>(opt => opt.UseNpgsql(connectionString));
        return services.BuildServiceProvider();
    }

    private static void InitializeDatabase(MainDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var ownerPermission = context.OrganizationRoles.FirstOrDefault(o => o.Name.Equals("OWNER"));
        var memberPermission = context.OrganizationRoles.FirstOrDefault(o => o.Name.Equals("MEMBER"));
        
        User user1 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        User user2 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"), "saras@gmail.com", "sara test", "sara", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        User user3 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test@gmail.com", "sara test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);

        var organization = Organization.Create("organization1", "contact@example.com", new List<User>());
        var organizationMember1 = OrganizationMember.Create(user1, organization, ownerPermission);
        var organizationMember2 = OrganizationMember.Create(user2, organization, memberPermission);

        organization.AddMember(organizationMember1);
        organization.AddMember(organizationMember2);
        
        // context.OrganizationRoles.AddRange(OrganizationRole.Owner(), OrganizationRole.Member());
        // context.OrganizationPermissions.AddRange(GetSeedPermissions());
        // context.OrganizationRolePermissions.AddRange(GetSeedPermissionRole());
        context.Users.AddRange(user1, user3);
        context.Organizations.Add(organization);
        context.SaveChanges();
    }
    
    private static List<OrganizationRolePermission> GetSeedPermissionRole()
        => new()
        {
            new("OWNER", "owner"),
            new("OWNER", "admin"),
            new("OWNER", "manager"),
            new("OWNER", "read_only"),
            new("MEMBER", "manager"),
            new("MEMBER", "read_only"),
        };
    private static List<OrganizationPermission> GetSeedPermissions()
        => new() { new("owner"), new("admin"), new("manager"), new("read_only") };

}