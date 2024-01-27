using Application.Notifications.Commands.UpdateNotificationPreferences;
using Domain.Auth;
using Domain.Auth.Enums;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Notifications;

[Collection("Sequential")]
public class UpdateNotificationPreferencesIntegrationTest : BaseIntegrationTest

{
    public UpdateNotificationPreferencesIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void UpdateNotificaionPreferences_ShouldBeSuccess()
    {
        //Arange   
        var command = new UpdateNotificationPreferencesCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            NotificationPreferences.DONTNOTIFY);

        //Act
        User user = await _sender.Send(command);

        //Assert
        user.NotificationPreferences.ShouldBeEquivalentTo(NotificationPreferences.DONTNOTIFY);
    }
    
}