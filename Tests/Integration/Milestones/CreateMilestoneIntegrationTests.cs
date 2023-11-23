using Application.Milestones.Commands.Create;
using Domain.Milestones;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Milestones;

[Collection("Sequential")]
public class CreateMilestoneIntegrationTests : BaseIntegrationTest
{
    public CreateMilestoneIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
       
    }
    
    [Fact]
    async Task CreateMilestone_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var command = new CreateMilestoneCommand("8e9b1cc2-35d3-4bf2-9f2c-9e00a21d94a5", "first milestone",
            "description",
            Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), new DateOnly());
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
            
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }
    
    [Fact]
    async Task CreateMilestone_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateMilestoneCommand("8e9b1cc2-35d3-4bf2-9f2c-9e00a21d94a5", "first milestone",
            "description",
            Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"), new DateOnly());
        //Act
      
        var milestoneId = await _sender.Send(command);
        Milestone milestone = await _context.Milestones.FindAsync(milestoneId);

        //Assert

        milestoneId.ShouldBeOfType<Guid>();
        milestone.ShouldNotBeNull();
    }
}