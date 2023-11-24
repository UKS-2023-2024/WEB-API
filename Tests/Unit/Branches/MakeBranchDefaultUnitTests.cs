using Application.Branches.Commands.Create;
using Application.Branches.Commands.Update;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Branches;

public class MakeBranchDefaultUnitTests
{
    private readonly Mock<IBranchRepository> _branchRepositoryMock;

    public MakeBranchDefaultUnitTests()
    {
        _branchRepositoryMock = new();
    }

    [Fact]
    public async void MakeBranchDefault_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new MakeBranchDefaultCommand(new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"));
        Branch branch1 = Branch.Create("name", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));

        _branchRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(branch1);
        var handler = new MakeBranchDefaultCommandHandler(_branchRepositoryMock.Object);

        //Act
        Branch branch = await handler.Handle(command, default);

        //Assert
        branch.IsDefault.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public async void MakeBranchDefault_ShouldFail_WhenBranchIsAlreadyDefault()
    {
        //Arrange
        var command = new MakeBranchDefaultCommand(new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"));
        Branch branch1 = Branch.Create("name", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), true, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));

        _branchRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(branch1);
        var handler = new MakeBranchDefaultCommandHandler(_branchRepositoryMock.Object);


        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<BranchIsAlreadyDefaultException>(() => handle());
    }

}