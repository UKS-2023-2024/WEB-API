using Application.Auth.Queries.FindAllByStarredRepository;
using Application.Repositories.Commands.StarringRepository.StarRepository;
using Application.Repositories.Commands.StarringRepository.UnstarRepository;
using Application.Repositories.Commands.WatchingRepository.UnwatchRepository;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Repositories.Exceptions;
using FluentResults;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;

[Collection("Sequential")]
public class UnwatchRepositoryIntegrationTest : BaseIntegrationTest
{
    public UnwatchRepositoryIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }

    [Fact]
    public async Task Unwatch_ShouldReturnSuccess_WhenUserWatchedRepositoryBefore()
    {
        //Arrange
        var user = _context.Users.FirstOrDefault(u => u.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        var command = new UnwatchRepositoryCommand(user!, new Guid("8e9b1cc1-35d3-4bf2-9f2c-5e00a21d14a5"));

        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.NotThrowAsync(Handle);
    }

    [Fact]
    public async Task Unwatch_ShouldReturnError_WhenUserIsNotWatchingRepository()
    {
        //Arrange
        var user = _context.Users.FirstOrDefault(u => u.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        var command = new UnwatchRepositoryCommand(user!, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"));
    
        //Act
        async Task Handle() => await _sender.Send(command);
    
        //Assert
        await Should.ThrowAsync<RepositoryNotWatchedException>(Handle);
    }
    
    [Fact]
    public async Task Unwatch_ShouldReturnError_WhenRepositoryDontExists()
    {
        //Arrange
        var user = _context.Users.FirstOrDefault(u => u.Id == new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
        var command = new UnwatchRepositoryCommand(user!, new Guid("8e9b1cc0-aaaa-aaaa-aaaa-5e00a21d1412"));
    
        //Act
        async Task Handle() => await _sender.Send(command);
    
        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
}