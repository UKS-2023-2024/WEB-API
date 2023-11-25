using Application.Branches.Commands.Create;
using Application.Milestones.Commands.Create;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Milestones;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Branches;

[Collection("Sequential")]
public class CreateBranchIntegrationTests : BaseIntegrationTest
{
    public CreateBranchIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {

    }

    [Fact]
    async Task CreateBranch_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateBranchCommand("grana", new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"), false, new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act
        var branchId = await _sender.Send(command);
        Branch? branch = await _context.Branches.FindAsync(branchId);

        //Assert
        branchId.ShouldBeOfType<Guid>();
        branch.ShouldNotBeNull();
    }
}