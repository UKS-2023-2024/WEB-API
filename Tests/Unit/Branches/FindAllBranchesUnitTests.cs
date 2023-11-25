using Application.Auth.Commands.Update;
using Application.Repositories.Queries.FindAllByOwnerId;
using Domain.Auth.Enums;
using Domain.Auth;
using Domain.Auth.Interfaces;
using Moq;
using Shouldly;
using Application.Repositories.Queries.FindAllByOrganizationId;
using Domain.Branches.Interfaces;
using Domain.Branches;
using Application.Repositories.Queries.FindAllNotDeletedByRepositoryId;

namespace Tests.Unit.Branches
{
    public class FindAllBranchesUnitTests
    {
        private Mock<IBranchRepository> _branchRepositoryMock;

        public FindAllBranchesUnitTests()
        {
            _branchRepositoryMock = new();
        }

        [Fact]
        async Task FindAllNotDeletedBranches_ShouldReturnNonEmptyList()
        {
            //Arrange
            var query = new FindAllNotDeletedBranchesByRepositoryIdQuery(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
            Branch branch1 = Branch.Create("branch1", new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), false, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d93a9"));
            Branch branch2 = Branch.Create("branch2", new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), false, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d93a9"));
            List<Branch> list = new List<Branch>() { branch1, branch2 };
            _branchRepositoryMock.Setup(x => x.FindAllNotDeletedByRepositoryId(It.IsAny<Guid>())).ReturnsAsync(list);

            var handler = new FindAllNotDeletedBranchesByRepositoryIdQueryHandler(_branchRepositoryMock.Object);

            //Act
            var branches = await handler.Handle(query, default);

            //Assert
            branches.ShouldNotBeEmpty();
        }

    }
}
