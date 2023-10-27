using Application.Auth.Commands.Register;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit;

public class RegistrationUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public RegistrationUnitTests()
    {
        _userRepositoryMock = new();
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_WhenEmailIsNotUnique()
    {
        //Arrange
        var command = new RegisterUserCommand("test@gmail.com",
            "123", "username", "firstName lastName");

        User foundUser = User.Create("email@gmail.com", "full name", "username", "password",
            UserRole.USER, null, null, null, null, null, null, false);

        _userRepositoryMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
            .Returns(foundUser);
        var handler = new RegisterUserCommandHandler(_userRepositoryMock.Object);
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        Should.ThrowAsync<Exception>(() => handle());
    }
    
    [Fact]
    public async void Handle_Should_ReturnSuccess_WhenEmailIsUnique()
    {
        //Arrange
        var command = new RegisterUserCommand("test@gmail.com",
            "123", "username", "firstName lastName");

        User foundUser = null;
        
        _userRepositoryMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
            .Returns(foundUser);
        var handler = new RegisterUserCommandHandler(_userRepositoryMock.Object);
        //Act
        string userId = await handler.Handle(command, default);

        //Assert
        userId.ShouldNotBeEmpty();
    }
    
    [Fact]
    public async void Handle_Should_ReturnFailure_WhenEmailIsNull()
    {
        //Arrange
        var command = new RegisterUserCommand(null,
            "123", "username", "firstName lastName");

        User foundUser = null;
        
        _userRepositoryMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
            .Returns(foundUser);
        var handler = new RegisterUserCommandHandler(_userRepositoryMock.Object);
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        Should.ThrowAsync<Exception>(() => handle());;
    }
    
    [Fact]
    public async void Handle_Should_ReturnFailure_WhenUsernameIsNull()
    {
        //Arrange
        var command = new RegisterUserCommand("email@gmail.com",
            "123", null, "firstName lastName");

        User foundUser = null;
        
        _userRepositoryMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
            .Returns(foundUser);
        var handler = new RegisterUserCommandHandler(_userRepositoryMock.Object);
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);
        };

        //Assert
        Should.ThrowAsync<Exception>(() => handle());;
    }
    
    [Fact]
    public async void Handle_Should_CallCreate_WhenEmailIsUniqueAndUsernameNotNull()
    {
        //Arrange
        var command = new RegisterUserCommand("email@gmail.com",
            "123456", "username", "firstName lastName");

        User foundUser = null;
        
        _userRepositoryMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
            .Returns(foundUser);
        var handler = new RegisterUserCommandHandler(_userRepositoryMock.Object);
        //Act
        string userId = await handler.Handle(command, default);

        //Assert
        _userRepositoryMock.Verify(x => x.Create(It.IsAny<User>()), Times.Once);
    }
}