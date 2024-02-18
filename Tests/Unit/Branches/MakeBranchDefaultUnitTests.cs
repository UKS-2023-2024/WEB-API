using Application.Branches.Commands.Create;
using Application.Branches.Commands.Update;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Branches.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Interfaces;
using Domain.Shared.Interfaces;
using Moq;
using Shouldly;

namespace Tests.Unit.Branches;

public class MakeBranchDefaultUnitTests
{
    private readonly Mock<IBranchRepository> _branchRepositoryMock= new ();
    private readonly Mock<IRepositoryRepository> _repositoryRepositoryMock= new();
    private readonly Mock<IGitService> _gitServiceMock = new ();
    private readonly Repository _repository1;


    public MakeBranchDefaultUnitTests()
    {
        var user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);

        _repository1 = Repository.Create(new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), "repository", "test", false, null, user1);
        _repositoryRepositoryMock.Setup(x => x.Find(_repository1.Id)).Returns(_repository1);
    }

    [Fact]
    public async void MakeBranchDefault_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new MakeBranchDefaultCommand(new Guid("705a6c69-5b51-4156-b4cc-71e8dd111579"));
        Branch branch1 = Branch.Create("name", new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), false, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));

        _branchRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(branch1);
        var handler = new MakeBranchDefaultCommandHandler(_branchRepositoryMock.Object,_repositoryRepositoryMock.Object,_gitServiceMock.Object);

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
        Branch branch1 = Branch.Create("name", new Guid("8e9b1cc1-ffaa-4bf2-9f2c-5e00a21d92a9"), true, new Guid("805a6c69-5b51-4156-b4cc-71e8dd111579"));

        _branchRepositoryMock.Setup(x => x.Find(It.IsAny<Guid>())).Returns(branch1);
        var handler = new MakeBranchDefaultCommandHandler(_branchRepositoryMock.Object,_repositoryRepositoryMock.Object,_gitServiceMock.Object);


        //Act
        Func<Task> handle = async () =>
        {
            await handler.Handle(command, default);

        };

        //Assert
        await Should.ThrowAsync<BranchIsAlreadyDefaultException>(() => handle());
    }

}