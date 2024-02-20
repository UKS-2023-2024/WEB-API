using Application.Branches.Commands.Create;
using Domain.Branches.Exceptions;
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
    async Task CreateBranch_ShouldFail_WhenBranchNameAlreadyExistsInRepository()
    {
        //Arrange
        var command = new CreateBranchCommand("branch1", new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"), false, new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),"branch2");
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<BranchWithThisNameExistsException>(Handle);
    }
    
    [Fact]
    async Task CreateBranch_ShouldFail_WhenBranchToCreateFromDoesNotExistInRepository()
    {
        //Arrange
        var command = new CreateBranchCommand("grana2", new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"), false, new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),"branchRandom");
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<BranchNotFoundException>(Handle);
    }

    [Fact]
    async Task CreateBranch_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateBranchCommand("grana1", new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"), false, new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),"branch2");
        //Act

        var branchId = await _sender.Send(command);
        var branch = await _context.Branches.FindAsync(branchId);

        //Assert
        branchId.ShouldBeOfType<Guid>();
        branch.ShouldNotBeNull();
    }


}

