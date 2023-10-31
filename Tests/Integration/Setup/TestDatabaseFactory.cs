using Domain.Auth;
using Domain.Auth.Enums;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
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

        services.AddDbContext<MainDbContext>(opt => opt.UseNpgsql(_configuration["ConnectionString"]));
        return services.BuildServiceProvider();
    }

    private static void InitializeDatabase(MainDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        var user = User.Create("test@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);

        context.Users.Add(user);
        context.SaveChanges();
    }
}