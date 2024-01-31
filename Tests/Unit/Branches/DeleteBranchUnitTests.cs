using Application.Branches.Commands.Create;
using Application.Branches.Commands.Delete;
using Application.Branches.Commands.Update;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Branches;

public class DeleteBranchUnitTests
{
    private readonly Mock<IBranchRepository> _branchRepositoryMock;
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock;
    private readonly Mock<IGitService> _gitServiceMock;

    public DeleteBranchUnitTests()
    {
        _branchRepositoryMock = new();
        _repositoryRepositoryMock = new();
        _gitServiceMock = new();
    }

    [Fact]
    public async void DeleteBranch_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new DeleteBranchCommand(new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"));
        Branch branch = Branch.Create("name", new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));

        _branchRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(branch);
        _branchRepositoryMock.Setup(x => x.Update(It.IsAny<Branch>()));

        var handler = new DeleteBranchCommandHandler(_branchRepositoryMock.Object,  _gitServiceMock.Object, _repositoryRepositoryMock.Object);

        //Act
        Branch deletedBranch = await handler.Handle(command, default);

        //Assert
        deletedBranch.Deleted.ShouldBeEquivalentTo(true);
    }

    [Fact]
    public async void DeleteBranch_ShouldFail_WhenBranchDoesntExist()
    {
        //Arrange
        var command = new DeleteBranchCommand(new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"));
        
        _branchRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns((Guid id) => null);

        var handler = new DeleteBranchCommandHandler(_branchRepositoryMock.Object, _gitServiceMock.Object, _repositoryRepositoryMock.Object);

        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<BranchNotFoundException>(() => handle());
    }

}