using Application.Milestones.Commands.Delete;
using Domain.Milestones.Exceptions;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Milestones;

[Collection("Sequential")]
public class DeleteMilestoneIntegrationTests: BaseIntegrationTest
{
    public DeleteMilestoneIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    async Task DeleteMilestone_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new DeleteMilestoneCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
        //Act
        var deletedMilestoneId = await _sender.Send(command);

        //Assert

        deletedMilestoneId.ShouldBeOfType<Guid>();
        deletedMilestoneId.ShouldBeEquivalentTo(Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"));
    }
    
    [Fact]
    async Task DeleteMilestone_ShouldFail_WhenMilestoneNotFound()
    {
        //Arrange
        var command = new DeleteMilestoneCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b4"));
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
            
        await Should.ThrowAsync<MilestoneNotFoundException>(() => handle());
    }
    
    [Fact]
    async Task DeleteMilestone_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var command = new DeleteMilestoneCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"),
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