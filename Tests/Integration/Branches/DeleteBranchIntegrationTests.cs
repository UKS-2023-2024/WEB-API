using Application.Branches.Commands.Delete;
using Domain.Branches.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Branches;

[Collection("Sequential")]
public class DeleteBranchIntegrationTests : BaseIntegrationTest
{
    public DeleteBranchIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {

    }

    [Fact]
    async Task DeleteBranch_ShouldFail_WhenBranchDoesntExist()
    {
        //Arrange
        var command = new DeleteBranchCommand(new Guid("8e9b1cc2-35d3-4bf2-9f2c-9e00a21d94a5"));
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert

        await Should.ThrowAsync<BranchNotFoundException>(() => handle());
    }

    [Fact]
    async Task DeleteBranch_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new DeleteBranchCommand(new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b1"));
        //Act

        var branch = await _sender.Send(command);

        //Assert
        branch.ShouldNotBeNull();
        branch.Deleted.ShouldBe(true);
    }


}

