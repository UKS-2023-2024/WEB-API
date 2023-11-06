using Application.Auth.Queries.Login;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Exceptions;
using Domain.Auth.Interfaces;
using Domain.Exceptions;
using Moq;
using Shouldly;

namespace Tests.Unit;

public class LoginUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IHashingService> _hashingService;
    
    public LoginUnitTests()
    {
        var user = User.Create("test@gmail.com", "full name", "username", "password",
            UserRole.USER);
        
        _userRepositoryMock = new Mock<IUserRepository>();
        _hashingService = new Mock<IHashingService>();
        
        _userRepositoryMock.Setup(x => x.FindUserByEmail(user.PrimaryEmail))
            .ReturnsAsync(user);
        _hashingService.Setup(x => x.Verify(user.Password,It.IsAny<string>()))
            .Returns(true);
    }
    [Fact]
    public async void Handle_ShouldReturnSuccess_WhenPasswordAndEmailMatchUser()
    {
        //Arrange
        var command = new LoginQuery("test@gmail.com",
            "password");
        
        var handler = new LoginQueryHandler(_userRepositoryMock.Object, _hashingService.Object);
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };
        //Assert
        await Should.NotThrowAsync(() => handle());
    }
    
    [Fact]
    public void Handle_ShouldThrowException_WhenEmailInvalid()
    {
        //Arrange
        var command = new LoginQuery("test123@gmail.com",
            "password");
        
        var handler = new LoginQueryHandler(_userRepositoryMock.Object, _hashingService.Object);
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };
        //Assert
        Should.ThrowAsync<InvalidCredentialsException>(() => handle());;
    }
    
    [Fact]
    public void Handle_ShouldThrowException_WhenPasswordInvalid()
    {
        //Arrange
        var command = new LoginQuery("test@gmail.com",
            "password123123");
        
        var handler = new LoginQueryHandler(_userRepositoryMock.Object, _hashingService.Object);
        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };
        //Assert
        Should.ThrowAsync<InvalidCredentialsException>(() => handle());;
    }
}