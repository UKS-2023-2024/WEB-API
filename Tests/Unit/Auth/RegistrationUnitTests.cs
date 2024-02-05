using Application.Auth.Commands.Register;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Domain.Shared.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit;

public class RegistrationUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IHashingService> _hashingService;
    private readonly Mock<IGitService> _gitService;

    public RegistrationUnitTests()
    {
        _userRepositoryMock = new();
        _hashingService = new();
        _gitService = new();
    }

    [Fact]
    public async void Handle_Should_ReturnFailure_WhenEmailIsNotUnique()
    {
        //Arrange
        var command = new RegisterUserCommand("test@gmail.com",
            "123", "username", "firstName lastName");

        User foundUser = User.Create("email@gmail.com", "full name", "username", "password",
            UserRole.USER, null, null, null, null, null, null, null);

        _userRepositoryMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(foundUser);
        var handler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _hashingService.Object, _gitService.Object);
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<UserWithThisEmailExistsException>(() => handle());
    }
    
    [Fact]
    public async void Handle_Should_ReturnSuccess_WhenEmailIsUnique()
    {
        //Arrange
        var command = new RegisterUserCommand("test@gmail.com",
            "123456", "username", "firstName lastName");

        User foundUser = null;
        User registeredUser = User.Create("test@gmail.com", "full name", "username", "password",
            UserRole.USER);
        
        _userRepositoryMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(foundUser);
        _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>())).ReturnsAsync(registeredUser);
        var handler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _hashingService.Object, _gitService.Object);
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };
       
        //Assert
        await Should.NotThrowAsync(() => handle());
    }
    
    [Fact]
    public async void Handle_Should_ReturnFailure_WhenEmailIsNull()
    {
        //Arrange
        var command = new RegisterUserCommand(null,
            "123", "username", "firstName lastName");

        User foundUser = null;
        
        _userRepositoryMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(foundUser);
        var handler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _hashingService.Object, _gitService.Object);
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);
        };

        //Assert
        Should.ThrowAsync<UserWithThisEmailExistsException>(() => handle());;
    }
    
    [Fact]
    public async void Handle_Should_ReturnFailure_WhenUsernameIsNull()
    {
        //Arrange
        var command = new RegisterUserCommand("email@gmail.com",
            "123", null, "firstName lastName");

        User foundUser = null;
        
        _userRepositoryMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(foundUser);
        var handler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _hashingService.Object, _gitService.Object);
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);
        };

        //Assert
        Should.ThrowAsync<UserWithThisEmailExistsException>(() => handle());;
    }
    
    [Fact]
    public async void Handle_Should_CallCreate_WhenEmailIsUniqueAndUsernameNotNull()
    {
        //Arrange
        var command = new RegisterUserCommand("email@gmail.com",
            "123456", "username", "firstName lastName");

        User foundUser = null;
        User registeredUser = User.Create("test@gmail.com", "full name", "username", "password",
            UserRole.USER);
        
        _userRepositoryMock.Setup(x => x.FindUserByEmail(It.IsAny<string>()))
            .ReturnsAsync(foundUser);
        _userRepositoryMock.Setup(x => x.Create(It.IsAny<User>()))
            .ReturnsAsync(registeredUser);
        var handler = new RegisterUserCommandHandler(_userRepositoryMock.Object, _hashingService.Object, _gitService.Object);
        //Act
        await handler.Handle(command, default);

        //Assert
        _userRepositoryMock.Verify(x => x.Create(It.IsAny<User>()), Times.Once);
    }
}