using Application.Repositories.Commands.StarringRepository.StarRepository;
using Application.Repositories.Commands.StarringRepository.UnstarRepository;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;

[Collection("Sequential")]
public class StarringRepositoriesIntegrationTest : BaseIntegrationTest
{
    public StarringRepositoriesIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async Task Handle_Unstar_ShouldReturnError_WhenUserDidntStarRepository()
    {
        //Arrange
        User user3 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test@gmail.com", "sara test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var command = new UnstarRepositoryCommand(user3, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"));
    
        //Act
        async Task Handle() => await _sender.Send(command);
    
        //Assert
        await Should.ThrowAsync<RepositoryNotStarredException>(Handle);
    }
    
    [Fact]
    public async Task Handle_Unstar_ShouldReturnError_WhenRepositoryDontExists()
    {
        //Arrange
        User user3 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test@gmail.com", "sara test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var command = new UnstarRepositoryCommand(user3, new Guid("8e9b1cc0-aaaa-aaaa-aaaa-5e00a21d1412"));
    
        //Act
        async Task Handle() => await _sender.Send(command);
    
        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }
    
    [Fact]
    public async Task Handle_Unstar_ShouldReturnSuccess_WhenUserStarredRepositoryBefore()
    {
        //Arrange
        User user3 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test@gmail.com", "sara test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var command = new UnstarRepositoryCommand(user3, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d14a5"));

        //Act
        async Task Handle() => await _sender.Send(command);
        
        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    [Fact]
    public async void Star_Handle_ShouldReturnSuccess_WhenRepositoryPublicAndUserDidntStar()
    {
        //Arrange
        User user3 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test@gmail.com", "sara test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var command = new StarRepositoryCommand(user3,
            new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"));
        
        //Act
        async Task Handle() => await _sender.Send(command);
        
        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenRepositoryPublicAndUserDidStar()
    {
        //Arrange
        User user3 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test@gmail.com", "sara test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var command = new StarRepositoryCommand(user3,
            new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d14a5"));
        
        //Act
        async Task Handle() => await _sender.Send(command);
        
        //Assert
        await Should.ThrowAsync<RepositoryAlreadyStarredException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenRepositoryPrivateAndUserNotMember()
    {
        //Arrange
        User user3 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test@gmail.com", "sara test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var command = new StarRepositoryCommand(user3,
            new Guid("8e9b1cc1-35d3-4bf2-9f2c-9e00a21d94a5"));
        
        //Act
        async Task Handle() => await _sender.Send(command);
        
        //Assert
        await Should.ThrowAsync<RepositoryInaccessibleException>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturnError_WhenRepositoryNotFound()
    {
        //Arrange
        User user3 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "test@gmail.com", "sara test", "dusan", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var command = new StarRepositoryCommand(user3,
            new Guid("8e9b1cc5-ffaa-aaaa-9f2c-5e00a21d92a9"));

        //Act
        async Task Handle() => await _sender.Send(command);
        
        //Assert
        await Should.ThrowAsync<RepositoryNotFoundException>(Handle);
    }

    [Fact]
    public async void Handle_ShouldReturnSuccess_WhenRepositoryPrivateAndUserMember()
    {
        //Arrange
        User user1 = User.Create(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), "anav@gmail.com", "test test", "test", "$2a$12$.33VvcDZ.ahQ0wEg3RMncurrbdUU0lkhyLQU2d1vVPXZlQSvgB5qq", UserRole.ADMINISTRATOR);
        var command = new StarRepositoryCommand(user1,
            new Guid("8e9b1cc1-35d3-4bf2-9f2c-9e00a21d94a5"));
        
        //Act
        async Task Handle() => await _sender.Send(command);
        
        //Assert
        await Should.NotThrowAsync(Handle);
    }
}