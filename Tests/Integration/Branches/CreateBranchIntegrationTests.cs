using Application.Branches.Commands.Create;
using Application.Milestones.Commands.Create;
using Domain.Branches;
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
        var command = new CreateBranchCommand("branch1", new Guid("8e9b1cc1-35d3-4bf2-9f2c-5e00a21d14a5"), false);

        //Act
        var branchId = await _sender.Send(command);
        Branch branch = await _context.Branches.FindAsync(branchId);

        //Assert
        branchId.ShouldBeOfType<Guid>();
        branch.ShouldNotBeNull();
    }
}