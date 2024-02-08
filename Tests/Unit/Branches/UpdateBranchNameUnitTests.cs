using Application.Branches.Commands.Create;
using Application.Branches.Commands.Update;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Branches;

public class UpdateBranchNameUnitTests
{
    private readonly Mock<IBranchRepository> _branchRepositoryMock;

    public UpdateBranchNameUnitTests()
    {
        _branchRepositoryMock = new();
    }

    [Fact]
    public async void UpdateBranchName_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new UpdateBranchNameCommand(new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), "changedName");
        Branch branch1 = Branch.Create("name", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));

        _branchRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(branch1);
        _branchRepositoryMock.Setup(x => x.FindByNameAndRepositoryId("", new Guid())).ReturnsAsync((string name, Guid repositoryId) => null); 
        var handler = new UpdateBranchNameCommandHandler(_branchRepositoryMock.Object);

        //Act
        Branch branch = await handler.Handle(command, default);

        //Assert
        branch.Name.ShouldBeEquivalentTo("changedName");
    }

    [Fact]
    public async void UpdateBranchName_ShouldFail_WhenBranchWithSameNameExistsInRepository()
    {
        //Arrange
        var command = new UpdateBranchNameCommand(new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), "changedName");
        Branch branch1 = Branch.Create("name", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111570"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111571"));
        branch1 = OverrideId<Branch>(branch1, new Guid("705a6c69-5b51-4156-b4cc-71e8dd111572"));
        Branch branch2 = Branch.Create("changedName", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111570"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111571"));
        branch2 = OverrideId<Branch>(branch2, new Guid("705a6c69-5b51-4156-b4cc-71e8dd111573"));
        _branchRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(branch1);
        _branchRepositoryMock.Setup(x => x.FindByNameAndRepositoryId("changedName", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111570"))).ReturnsAsync(branch2);
        var handler = new UpdateBranchNameCommandHandler(_branchRepositoryMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<BranchWithThisNameExistsException>(() => handle());
    }
    
    private T OverrideId<T>(T obj, Guid id)
    {
        var propertyInfo = typeof(T).GetProperty("Id");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, id);
        return obj;
    }

}