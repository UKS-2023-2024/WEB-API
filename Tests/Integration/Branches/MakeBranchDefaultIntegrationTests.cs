using Application.Branches.Commands.Delete;
using Application.Branches.Commands.Update;
using Domain.Branches.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Branches;

[Collection("Sequential")]
public class MakeBranchDefaultIntegrationTests : BaseIntegrationTest
{
    public MakeBranchDefaultIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {

    }

    [Fact]
    async Task MakeBranchDefaultBranch_ShouldFail_WhenBranchIsAlreadyDefault()
    {
        //Arrange
        var command = new MakeBranchDefaultCommand(new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b1"));
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert

        await Should.ThrowAsync<BranchIsAlreadyDefaultException>(() => handle());
    }

    [Fact]
    async Task MakeBranchDefault_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new MakeBranchDefaultCommand(new Guid("8e9b1cc3-36d3-4bf2-9f2c-9e00a21d94b2"));
        //Act

        var branch = await _sender.Send(command);

        //Assert
        branch.ShouldNotBeNull();
        branch.IsDefault.ShouldBe(true);
    }


}

