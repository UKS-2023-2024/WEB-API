using Application.Issues.Commands.Enums;
using Application.Issues.Commands.Update;
using Domain.Repositories;
using Domain.Tasks;
using Domain.Tasks.Enums;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using Tests.Integration.Setup;
using Task = System.Threading.Tasks.Task;

namespace Tests.Integration.Issues;

[Collection("Sequential")]
public class AssignUserToUserIntegrationTests: BaseIntegrationTest
{
    public AssignUserToUserIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    async Task AssignIssueToUser_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange



        var repositoryId = Guid.Parse("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5");
        var authorized = Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
         var assignees = new List<string>();
         assignees.Add("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
         var existingIssue = _context.Issues.First();
         var assigneeGuids = assignees.Select(Guid.Parse);
         
         var command = new UpdateIssueCommand(existingIssue.Id, authorized, It.IsAny<string>(), It.IsAny<string>(),
             It.IsAny<TaskState>(), It.IsAny<int>(), repositoryId,
             assignees, It.IsAny<List<string>>(), UpdateIssueFlag.ASSIGNEES, It.IsAny<Guid>());

         //Act
         var issueId = await _sender.Send(command);
         Issue? issue = await _context.Issues.Where(i => i.Id.Equals(issueId))
             .Include(i => i.Assignees)
             .Include(i => i.Labels)
             .FirstOrDefaultAsync();
         //Assert
         issueId.ShouldBeOfType<Guid>();
         issue.ShouldNotBeNull();
         issue.Assignees.ShouldNotBeEmpty();
    }
}