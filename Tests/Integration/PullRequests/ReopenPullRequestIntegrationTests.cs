using Application.Milestones.Commands.Close;
using Application.PullRequests.Commands.Close;
using Application.PullRequests.Commands.Reopen;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Repositories.Exceptions;
using Domain.Tasks.Enums;
using Domain.Tasks.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.PullRequests;

[Collection("Sequential")]
public class ReopenPullRequestIntegrationTests: BaseIntegrationTest
{
    public ReopenPullRequestIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    async Task ReopenPullRequest_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var closeCommand = new ClosePullRequestCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), new Guid("8e9b1cc3-36d3-4bf2-9f4c-9e00a21d94b3"));
        var reopenCommand = new ReopenPullRequestCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), new Guid("8e9b1cc3-36d3-4bf2-9f4c-9e00a21d94b3"));
        //Act
        var closedPr = await _sender.Send(closeCommand);
        var reopenedPr = await _sender.Send(reopenCommand);
        //Assert

        reopenedPr.ShouldBeOfType<Domain.Tasks.PullRequest>();
        reopenedPr.State.ShouldBeEquivalentTo(TaskState.OPEN);
    }
    
    [Fact]
    async Task ReopenPullRequest_ShouldFail_WhenPullRequestNotFound()
    {
        //Arrange
        var command = new ClosePullRequestCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            new Guid("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b4"));
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
            
        await Should.ThrowAsync<PullRequestNotFoundException>(() => handle());
    }
    
    [Fact]
    async Task ReopenPullRequest_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var command = new ClosePullRequestCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9213"), new Guid("8e9b1cc3-36d3-4bf2-9f4c-9e00a21d94b3"));
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }
}