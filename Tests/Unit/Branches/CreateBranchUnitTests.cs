using Application.Branches.Commands.Create;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Branches;

public class CreateBranchUnitTests
{
    private readonly Mock<IBranchRepository> _branchRepositoryMock = new();
    private readonly Mock<IGitService> _gitService = new();
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock = new();

    public CreateBranchUnitTests()
    {
        var branch1 = Branch.Create("branch1", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));
        branch1 = OverrideId<Branch>(branch1,new Guid("705a6c69-5aaa-4156-b4cc-71e8dd111579"));
        var branch2 = Branch.Create("branch2", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));
        branch2 = OverrideId<Branch>(branch2,new Guid("705a6c69-5bbb-4156-b4cc-71e8dd111579"));
        
        _branchRepositoryMock.Setup(x => x.FindByNameAndRepositoryId(branch1.Name, branch1.RepositoryId))
            .ReturnsAsync(branch1);
        _branchRepositoryMock.Setup(x => x.FindByNameAndRepositoryId(branch2.Name, branch2.RepositoryId))
            .ReturnsAsync(branch2);
    }

    [Fact]
    public async void CreateBranch_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateBranchCommand("branch3", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"), "branch1");
        var branch3 = Branch.Create("branch1", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));
        branch3 = OverrideId<Branch>(branch3,new Guid("705a6c69-5ccc-4156-b4cc-71e8dd111579"));
        _branchRepositoryMock.Setup(x => x.Create(It.IsAny<Branch>())).ReturnsAsync(branch3);
        var handler = new CreateBranchCommandHandler(_branchRepositoryMock.Object, _gitService.Object, _repositoryRepositoryMock.Object);

        //Act
        var result = handler.Handle(command, default);

        //Assert
        result.IsFaulted.ShouldBe(false);
    }

    [Fact]
    public async void CreateBranch_ShouldFail_WhenBranchWithSameNameExistsInRepository()
    {
        //Arrange
        var command = new CreateBranchCommand("branch1", Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"), null);
        var handler = new CreateBranchCommandHandler(_branchRepositoryMock.Object, _gitService.Object, _repositoryRepositoryMock.Object);

        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<BranchWithThisNameExistsException>(Handle);
    }
    
    [Fact]
    public async void CreateBranch_ShouldFail_WhenBranchToCreateFromDoesNotExistInRepository()
    {
        //Arrange
        var command = new CreateBranchCommand("branchRandom", Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"), null);
        var handler = new CreateBranchCommandHandler(_branchRepositoryMock.Object, _gitService.Object, _repositoryRepositoryMock.Object);

        //Act
        async Task Handle() => await handler.Handle(command, default);

        //Assert
        await Should.ThrowAsync<BranchNotFoundException>(Handle);
    }
    
    private T OverrideId<T>(T obj, Guid id)
    {
        var propertyInfo = typeof(T).GetProperty("Id");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, id);
        return obj;
    }

}