using Application.Milestones.Commands.Close;
using Application.PullRequests.Commands.Close;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Repositories.Exceptions;
using Domain.Tasks.Enums;
using Domain.Tasks.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.PullRequests;

[Collection("Sequential")]
public class ClosePullRequestIntegrationTests: BaseIntegrationTest
{
    public ClosePullRequestIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    async Task ClosePullRequest_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new ClosePullRequestCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), new Guid("8e9b1cc3-36d3-4bf2-9f4c-9e00a21d94b3"));
        //Act
        var closedPr = await _sender.Send(command);

        //Assert

        closedPr.ShouldBeOfType<Domain.Tasks.PullRequest>();
        closedPr.State.ShouldBeEquivalentTo(TaskState.CLOSED);
    }
    
    [Fact]
    async Task ClosePullRequest_ShouldFail_WhenPullRequestNotFound()
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
    async Task ClosePullRequest_ShouldFail_WhenUserNotRepositoryMember()
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