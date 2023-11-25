using Application.Branches.Commands.Create;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Branches;

public class CreateBranchUnitTests
{
    private readonly Mock<IBranchRepository> _branchRepositoryMock;

    public CreateBranchUnitTests()
    {
        _branchRepositoryMock = new();
    }

    [Fact]
    public async void CreateBranch_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateBranchCommand("branch1", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));
        Branch branch = Branch.Create("branch1", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));
       
        _branchRepositoryMock.Setup(x => x.Create(It.IsAny<Branch>()))
            .ReturnsAsync(branch);
   
        var handler = new CreateBranchCommandHandler(_branchRepositoryMock.Object);

        //Act
        Guid branchId = await handler.Handle(command, default);

        //Assert
        branchId.ShouldBeOfType<Guid>();
    }

    [Fact]
    public async void CreateBranch_ShouldFail_WhenBranchWithSameNameExistsInRepository()
    {
        //Arrange
        var command = new CreateBranchCommand("branch", Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));
        Branch branch = Branch.Create("branch", Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));

        _branchRepositoryMock.Setup(x => x.FindByNameAndRepositoryId("branch", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579")))
            .ReturnsAsync(branch);

        var handler = new CreateBranchCommandHandler(_branchRepositoryMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<BranchWithThisNameExistsException>(() => handle());
    }

}