﻿using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Branches;
using Domain.Milestones;
using Domain.Organizations;
using Domain.Repositories;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        services.AddDbContext<MainDbContext>(opt => opt.UseNpgsql(_configuration["TestConnectionString"]));
        return services.BuildServiceProvider();
    }

    private static void InitializeDatabase(MainDbContext context)
    {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        
        
        var user1 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var user2 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"), "saras@gmail.com", "sara test", "sara", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var user3 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test@gmail.com", "sara test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var user4 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9211"), "dusan@gmail.com", "dusan test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.USER);
        var user5 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9213"), "dusan@gmail.com", "dusan test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.USER);

        var organization1 = Organization.Create("organization1", "contact@example.com", new List<User>(),user1);
        var organization2 = Organization.Create("organization2", "contact2@example.com", new List<User>(),user1);
        
        var organizationMember = organization1.AddMember(user2);
        organizationMember.SetRole(OrganizationMemberRole.MODERATOR);
        organization1.AddMember(user4);

        var repository1 = Repository.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), "repo", "test", false, null, user1);
        var repositoryMember = repository1.AddMember(user2);
        repositoryMember.Id = new Guid("11111cc0-35d3-4bf2-9f2c-5e00a21d9111");
        repository1.AddMember(user1);
        
        var repository2 = Repository.Create(new Guid("8e9b1cc1-35d3-4bf2-9f2c-5e00a21d14a5"), "repo2", "test", false, organization1, user1);
        repository2.AddToStarredBy(user3);
        repository2.AddToStarredBy(user4);
        repository2.AddToWatchedBy(user3);
        repository2.AddToWatchedBy(user4);

        var repository3 = Repository.Create(new Guid("8e9b1cc2-35d3-4bf2-9f2c-9e00a21d94a5"), "repo3", "test", false, null, user1);
        var member1 = repository3.AddMember(user2);
        repository3.AddMember(user3);
        var member3 = repository3.AddMember(user4);
        member1.Delete();
        member3.SetRole(RepositoryMemberRole.ADMIN);
        
        var repository4 = Repository.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"), "repo4", "test", true, null, user1);
        var member2 = repository3.AddMember(user2);
        member2.Delete(); 

        var repository5 = Repository.Create(new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"), "repo5", "test", true, organization1, user1);
        repository5.AddToStarredBy(user1);
        repository5.AddToWatchedBy(user1);
        var milestone1 = Milestone.Create(new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"), "title", "description", new DateOnly(), new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"));

        var branch1 = Branch.Create(new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b1"), "branch1", repository5.Id, true, user1.Id);
        var branch2 = Branch.Create(new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b2"), "branch2", repository5.Id, false, user1.Id);
        var branch3 = Branch.Create(new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b3"), "branch3", repository5.Id, false, user1.Id, true);

        var label1 = new Label("label1", "123", "", Guid.Parse("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"));
        
        var issue1 = Issue.Create("first issue", "description", TaskState.OPEN, 1, repository1,
            user1, new List<RepositoryMember>(), new List<Label>() {label1}, null);
        
        context.Users.AddRange(user1, user2, user3, user4);
        context.Organizations.AddRange(organization1,organization2);
        context.Repositories.AddRange(repository1,repository2,repository3,repository4,repository5);
        context.Milestones.AddRange(milestone1);
        context.Branches.AddRange(branch1, branch2, branch3);
        context.Issues.AddRange(issue1);
        context.SaveChanges();
    }

}