using Application.Repositories.Queries.FindAllUserWithoutDefaultByRepositoryId;
using Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryId;
using Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryIdPagination;
using Application.Repositories.Queries.FindDefaultBranchByRepositoryId;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Branches;

[Collection("Sequential")]
public class FindBranchesIntegrationTests : BaseIntegrationTest
{
    public FindBranchesIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {

    }

    [Fact]
    public async Task FindAllBranchesWithoutDefaultByRepositoryIdPagination_ShouldReturnNonEmptyList()
    {
        //Arrange
        var query = new FindAllBranchesWithoutDefaultByRepositoryIdPaginationQuery(new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"), 10, 1);

        //Act
        var branches = await _sender.Send(query);

        //Assert
        branches.Data.ShouldNotBeEmpty();
    }


    [Fact]
    public async Task FindAllBranchesWithoutDefaultByRepositoryId_ShouldReturnNonEmptyList()
    {
        //Arrange
        var query = new FindAllBranchesWithoutDefaultByRepositoryIdQuery(new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"));

        //Act
        var branches = await _sender.Send(query);

        //Assert
        branches.ShouldNotBeEmpty();
    }


    [Fact]
    public async Task FindAllUserBranchesWithoutDefaultByRepositoryId_ShouldReturnNonEmptyList()
    {
        //Arrange
        var query = new FindAllUserBranchesWithoutDefaultByRepositoryIdQuery(new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"), new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), 10, 1);

        //Act
        var branches = await _sender.Send(query);

        //Assert
        branches.Data.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task FindDefaultBranchByRepositoryId_ShouldNotReturnNull()
    {
        //Arrange
        var query = new FindDefaultBranchByRepositoryIdQuery(new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"));

        //Act
        var branch = await _sender.Send(query);

        //Assert
        branch.ShouldNotBeNull();
        branch.IsDefault.ShouldBeTrue();
    }


}

