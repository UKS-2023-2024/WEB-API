using Application.Branches.Commands.Create;
using Application.Milestones.Commands.Create;
using Domain.Auth;
using Domain.Branches;
using Domain.Branches.Interfaces;
using Domain.Milestones;
using Domain.Milestones.Interfaces;
using Domain.Repositories;
using Domain.Repositories.Exceptions;
using Domain.Repositories.Interfaces;
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
        var command = new CreateBranchCommand("branch1", Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), false);
        Branch branch = Branch.Create("branch1", Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"), false);
       
        _branchRepositoryMock.Setup(x => x.Create(It.IsAny<Branch>()))
            .ReturnsAsync(branch);
   
        var handler = new CreateBranchCommandHandler(_branchRepositoryMock.Object);

        //Act
        Guid branchId = await handler.Handle(command, default);

        //Assert
        branchId.ShouldBeOfType<Guid>();
    }

}