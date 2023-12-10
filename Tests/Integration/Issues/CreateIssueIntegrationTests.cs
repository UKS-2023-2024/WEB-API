using Application.Issues.Commands.Create;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Tasks;
using Shouldly;
using Tests.Integration.Setup;
using Task = System.Threading.Tasks.Task;

namespace Tests.Integration.Issues;

[Collection("Sequential")]
public class CreateIssueIntegrationTests: BaseIntegrationTest
{
    public CreateIssueIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }

    [Fact]
    async Task CreateIssue_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateIssueCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"), "first issue",
            "description",
            Guid.Parse("8e9b1cc2-35d3-4bf2-9f2c-9e00a21d94a5"), new List<RepositoryMember>(), new List<Label>(), null);
        
        //Act
        var issueId = await _sender.Send(command);
        Issue issue = await _context.Issues.FindAsync(issueId);
        
        //Assert
        issueId.ShouldBeOfType<Guid>();
        issue.ShouldNotBeNull();
    }
    
    [Fact]
    async Task CreateMilestone_ShouldFail_WhenUserNotRepositoryMember()
    {
        //Arrange
        var command = new CreateIssueCommand(Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a1"), "first issue",
            "description",
            Guid.Parse("8e9b1cc2-35d3-4bf2-9f2c-9e00a21d94a5"), new List<RepositoryMember>(), new List<Label>(), null);
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
            
        await Should.ThrowAsync<RepositoryMemberNotFoundException>(() => handle());
    }
}