using Application.Branches.Commands.Update;
using Domain.Branches.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Branches;

[Collection("Sequential")]
public class UpdateBranchIntegrationTests : BaseIntegrationTest
{
    public UpdateBranchIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {

    }

    [Fact]
    async Task UpdateBranchName_ShouldFail_WhenBranchNameAlreadyExistsInRepository()
    {
        //Arrange
        var command = new UpdateBranchNameCommand(new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b2"), "branch1");
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert

        await Should.ThrowAsync<BranchWithThisNameExistsException>(() => handle());
    }

    [Fact]
    async Task UpdateBranchName_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new UpdateBranchNameCommand(new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b2"), "newBranchName");
        //Act

        var branch = await _sender.Send(command);

        //Assert
        branch.ShouldNotBeNull();
        branch.Name.ShouldBe("newBranchName");
    }


}

