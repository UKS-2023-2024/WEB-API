using Application.Notifications.Commands.UpdateNotificationPreferences;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Notifications;

public class UpdateNotificationPreferencesUnitTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;

    public UpdateNotificationPreferencesUnitTests()
    {
        _userRepositoryMock = new();
    }

    [Fact]
    public async void UpdateBranchName_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new UpdateNotificationPreferencesCommand(new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), NotificationPreferences.GITHUB);
        User user = User.Create("primary email", "full name", "username", "password", UserRole.USER);
        _userRepositoryMock.Setup(x => x.FindUserById(It.IsAny<Guid>())).ReturnsAsync(user);

        var handler = new UpdateNotificationPreferencesCommandHandler(_userRepositoryMock.Object);

        //Act
        User userRet = await handler.Handle(command, default);

        //Assert
        userRet.NotificationPreferences.ShouldBeEquivalentTo(NotificationPreferences.GITHUB);
    }

   
}