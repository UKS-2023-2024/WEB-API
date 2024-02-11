using Application.Milestones.Commands.Close;
using Application.Milestones.Commands.Reopen;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Milestones;

[Collection("Sequential")]
public class ReopenMilestoneIntegrationTests: BaseIntegrationTest
{
    public ReopenMilestoneIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    async Task ReopenMilestone_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var closeCommand = new CloseMilestoneCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        var command = new ReopenMilestoneCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        //Act
        var closedMilestone = await _sender.Send(closeCommand);
        var reopenedMilestone = await _sender.Send(command);
        
        //Assert

        reopenedMilestone.ShouldBeOfType<Milestone>();
        reopenedMilestone.Closed.ShouldBeFalse();
    }
    
    [Fact]
    async Task ReopenMilestone_ShouldFail_WhenMilestoneNotFound()
    {
        //Arrange
        var command = new ReopenMilestoneCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b9"));
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
            
        await Should.ThrowAsync<MilestoneNotFoundException>(() => handle());
    }
    
    [Fact]
    async Task ReopenMilestone_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var command = new ReopenMilestoneCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }
}