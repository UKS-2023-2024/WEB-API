using Shouldly;
using Domain.Branches.Interfaces;
using Domain.Branches;
using Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryId;
using Application.Repositories.Queries.FindAllUserWithoutDefaultByRepositoryId;
using Domain.Shared;
using Moq;
using Application.Repositories.Queries.FindDefaultBranchByRepositoryId;
using Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryIdPagination;

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
        async Task FindAllBranchesWithoutDefault_ShouldReturnNonEmptyList()
        {
            //Arrange
            var query = new FindAllBranchesWithoutDefaultByRepositoryIdQuery(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
            Branch branch1 = Branch.Create("branch1", new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), false, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d93a9"));
            Branch branch2 = Branch.Create("branch2", new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), false, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d93a9"));
            List<Branch> list = new List<Branch>() { branch1, branch2 };
            _branchRepositoryMock.Setup(x => x.FindAllByRepositoryIdAndIsDefault(It.IsAny<Guid>(), false)).ReturnsAsync(list);

            var handler = new FindAllBranchesWithoutDefaultByRepositoryIdQueryHandler(_branchRepositoryMock.Object);

            //Act
            var branches = await handler.Handle(query, default);

            //Assert
            branches.ShouldNotBeEmpty();
        }

        [Fact]
        async Task FindAllBranchesWithoutDefaultPagination_ShouldReturnNonEmptyList()
        {
            //Arrange
            var query = new FindAllBranchesWithoutDefaultByRepositoryIdPaginationQuery(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), 10, 1);
            Branch branch1 = Branch.Create("branch1", new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), false, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d93a9"));
            Branch branch2 = Branch.Create("branch2", new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), false, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d93a9"));
            List<Branch> list = new List<Branch>() { branch1, branch2 };
            _branchRepositoryMock.Setup(x => x.FindAllByRepositoryIdAndDeletedAndIsDefault(It.IsAny<Guid>(), false, false, 10, 1)).ReturnsAsync(new PagedResult<Branch>(list, list.Count));

            var handler = new FindAllBranchesWithoutDefaultByRepositoryIdPaginationQueryHandler(_branchRepositoryMock.Object);

            //Act
            var branches = await handler.Handle(query, default);

            //Assert
            branches.Data.ShouldNotBeEmpty();
            branches.TotalItems.ShouldBeEquivalentTo(2);
        }

        [Fact]
        async Task FindAllUserBranchesWithoutDefault_ShouldReturnNonEmptyList()
        {
            //Arrange
            var query = new FindAllUserBranchesWithoutDefaultByRepositoryIdQuery(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d93a9"), 10, 1);
            Branch branch1 = Branch.Create("branch1", new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), false, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d93a9"));
            Branch branch2 = Branch.Create("branch2", new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), false, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d93a9"));
            List<Branch> list = new List<Branch>() { branch1, branch2 };
            _branchRepositoryMock.Setup(x => x.FindAllByRepositoryIdAndOwnerIdAndDeletedAndIsDefault(It.IsAny<Guid>(), It.IsAny<Guid>(), false, false, 10, 1)).ReturnsAsync(new PagedResult<Branch>(list, list.Count));

            var handler = new FindAllUserBranchesWithoutDefaultByRepositoryIdQueryHandler(_branchRepositoryMock.Object);

            //Act
            var branches = await handler.Handle(query, default);

            //Assert
            branches.Data.ShouldNotBeEmpty();
            branches.TotalItems.ShouldBe(2);
        }

        [Fact]
        async Task FindDefaultBranch_ShouldReturnNonEmptyList()
        {
            //Arrange
            var query = new FindDefaultBranchByRepositoryIdQuery(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"));
            Branch branch = Branch.Create("branch1", new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), true, new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d93a9"));
            _branchRepositoryMock.Setup(x => x.FindByRepositoryIdAndIsDefault(It.IsAny<Guid>(), true)).ReturnsAsync(branch);

            var handler = new FindDefaultBranchByRepositoryIdQueryHandler(_branchRepositoryMock.Object);

            //Act
            var defaultBranch = await handler.Handle(query, default);

            //Assert
            defaultBranch.ShouldNotBeNull();
            defaultBranch.IsDefault.ShouldBeEquivalentTo(true);
        }

    }
}
