using Application.Auth.Commands.SetPublicKey;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;

namespace Tests.Unit.Auth;

public class SetPublicKeyUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IGitService> _gitService;

    public SetPublicKeyUnitTests()
    {
        _userRepositoryMock = new();
        _gitService = new();
    }


    [Fact]
    public async Task Handle_ShouldPass_WhenUserExists()
    {
        
        //Arrange
        var command = new SetPublicKeyCommand(new Guid(), "pk");
        var user = User.Create(
            new Guid(), "", "", "", "", UserRole.USER);
        _userRepositoryMock.Setup(u => u.FindUserById(It.IsAny<Guid>()))
            .ReturnsAsync(user);
        _gitService.Setup(u => u.SetPublicKey(It.IsAny<User>(), It.IsAny<string>()))
            .Verifiable();
        var handler = new SetPublicKeyCommandHandler(_userRepositoryMock.Object, _gitService.Object);
        
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);
        };
        //Assert
        await Should.NotThrowAsync(() => handle());
    }
    
    [Fact]
    public async Task Handle_ShouldFail_WhenUserDoesntExists()
    {
        
        //Arrange
        var command = new SetPublicKeyCommand(new Guid(), "pk");
        _userRepositoryMock.Setup(u => u.FindUserById(It.IsAny<Guid>()))
            .ReturnsAsync((User?)null);
        _gitService.Setup(u => u.SetPublicKey(It.IsAny<User>(), It.IsAny<string>()))
            .Verifiable();
        var handler = new SetPublicKeyCommandHandler(_userRepositoryMock.Object, _gitService.Object);
        
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);
        };
        //Assert
        await Should.ThrowAsync<UserNotFoundException>(() => handle());
    }
}