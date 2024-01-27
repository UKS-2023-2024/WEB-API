using Application.Auth.Queries.FindAllByStarredRepository;
using Application.Repositories.Commands.StarringRepository.StarRepository;
using Application.Repositories.Commands.StarringRepository.UnstarRepository;
using Application.Repositories.Commands.WatchingRepository.WatchRepository;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Repositories.Enums;
using Domain.Repositories.Exceptions;
using FluentResults;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;

[Collection("Sequential")]
public class WatchRepositoryIntegrationTest : BaseIntegrationTest
{
    public WatchRepositoryIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }

    [Fact]
    public async void Watch_ShouldReturnSuccess_WhenRepositoryPublic()
    {
        //Arrange
        var user = _context.Users.FirstOrDefault(u => u.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        var command = new WatchRepositoryCommand(user!, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), WatchingPreferences.AllActivity);
        
        //Act
        async Task Handle() => await _sender.Send(command);
        
        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    
    [Fact]
    public async void Watch_ShouldReturnError_WhenRepositoryPrivateAndUserNotMember()
    {
        //Arrange
        var user = _context.Users.FirstOrDefault(u => u.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        var command = new WatchRepositoryCommand(user!, new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"), WatchingPreferences.AllActivity);
        
        //Act
        async Task Handle() => await _sender.Send(command);
        
        //Assert
        await Should.ThrowAsync<RepositoryInaccessibleException>(Handle);
    }
    
    [Fact]
    public async void Watch_ShouldReturnError_WhenRepositoryNotFound()
    {
        //Arrange
        var user = _context.Users.FirstOrDefault(u => u.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        var command = new WatchRepositoryCommand(user!, new Guid("8e9b1cc5-ffaa-aaaa-9f2c-5e00a21d92a9"), WatchingPreferences.AllActivity);

        //Act
        async Task Handle() => await _sender.Send(command);
        
        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }

    [Fact]
    public async void Watch_ShouldReturnSuccess_WhenRepositoryPrivateAndUserMember()
    {
        //Arrange
        var user = _context.Users.FirstOrDefault(u => u.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
        var command = new WatchRepositoryCommand(user!, new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94a5"), WatchingPreferences.AllActivity);
        
        //Act
        async Task Handle() => await _sender.Send(command);
        
        //Assert
        await Should.NotThrowAsync(Handle);
    }
}