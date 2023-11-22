using Application.Milestones.Commands.Update;
using Domain.Milestones;
using Domain.Milestones.Exceptions;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Milestones;

[Collection("Sequential")]
public class UpdateMilestoneIntegrationTests : BaseIntegrationTest
{
    public UpdateMilestoneIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    async Task UpdateMilestone_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new UpdateMilestoneCommand(Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"),
            "updated title", "updated description", Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), new DateOnly());
        //Act
        var updatedMilestone = await _sender.Send(command);

        //Assert

        updatedMilestone.ShouldBeOfType<Milestone>();
        updatedMilestone.ShouldNotBeNull();
        updatedMilestone.Description.ShouldBeEquivalentTo("updated description");
        updatedMilestone.Title.ShouldBeEquivalentTo("updated title");
    }
    
    [Fact]
    async Task UpdateMilestone_ShouldFail_WhenMilestoneNotFound()
    {
        //Arrange
        var command = new UpdateMilestoneCommand(Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b7"),
            "updated title", "updated description", Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), new DateOnly());
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
        var command = new UpdateMilestoneCommand(Guid.Parse("8e9b1cc3-35d3-4bf2-9f2c-9e00a21d94b3"),
            "updated title", "updated description", Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), new DateOnly());
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }
}