using Application.Notifications.Queries.FindUserNotifications;
using Application.Repositories.Queries.FindAllUserWithoutDefaultByRepositoryId;
using Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryId;
using Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryIdPagination;
using Application.Repositories.Queries.FindDefaultBranchByRepositoryId;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Notifications;

[Collection("Sequential")]
public class FindUserNotificationsIntegrationTests : BaseIntegrationTest
{
    public FindUserNotificationsIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {

    }

    [Fact]
    public async Task FindUserNotifications_ShouldReturnNonEmptyList()
    {
        //Arrange
        var query = new FindUserNotificationsQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act
        var data = await _sender.Send(query);

        //Assert
        data.ShouldNotBeEmpty();
    }

}

