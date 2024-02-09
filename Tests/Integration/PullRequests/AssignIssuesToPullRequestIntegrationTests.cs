using Application.Milestones.Commands.Close;
using Application.PullRequests.Commands.Close;
using Application.PullRequests.Commands.Update;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Repositories.Exceptions;
using Domain.Tasks.Enums;
using Domain.Tasks.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.PullRequests;

[Collection("Sequential")]
public class AssignIssuesToPullRequestIntegrationTests : BaseIntegrationTest
{
    public AssignIssuesToPullRequestIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }

    [Fact]
    async Task AssignIssuesToPullRequest_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new AssignIssuesToPullRequestCommand(new Guid("8e9b1cc3-36d3-4bf2-9f4c-9e00a21d94b3"), new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5")
            , new List<Guid>() { new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21c94b5") });
        //Act
        var prId = await _sender.Send(command);

        //Assert

        prId.ShouldBeOfType<Guid>();
    }

    [Fact]
    async Task AssignIssuesToPullRequest_ShouldFail_WhenPullRequestNotFound()
    {
        //Arrange
        var command = new AssignIssuesToPullRequestCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b4"), new List<Guid>() { });
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert

        await Should.ThrowAsync<PullRequestNotFoundException>(() => handle());
    }

    [Fact]
    async Task AssignIssueToPullRequest_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var command = new AssignIssuesToPullRequestCommand(new Guid("8e9b1cc3-36d3-4bf2-9f4c-9e00a21d94b3"), new Guid("7e9b1bb0-35d3-4bf2-9f2c-5e00a21d92a5")
            , new List<Guid>() { });
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }
}