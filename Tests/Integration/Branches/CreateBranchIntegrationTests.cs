using Application.Branches.Commands.Create;
using Application.Milestones.Commands.Create;
using Application.Repositories.Queries.FindAllUserWithoutDefaultByRepositoryId;
using Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryId;
using Application.Repositories.Queries.FindAllWithoutDefaultByRepositoryIdPagination;
using Application.Repositories.Queries.FindDefaultBranchByRepositoryId;
using Domain.Branches;
using Domain.Branches.Exceptions;
using Domain.Milestones;
using Domain.Repositories.Exceptions;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Integration.Setup;

namespace Tests.Integration.Branches;

[Collection("Sequential")]
public class CreateBranchIntegrationTests : BaseIntegrationTest
{
    public CreateBranchIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {

    }

    [Fact]
    async Task CreateBranch_ShouldFail_WhenBranchNameAlreadyExistsInRepository()
    {
        //Arrange
        var command = new CreateBranchCommand("branch1", new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"), false, new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert

        await Should.ThrowAsync<BranchWithThisNameExistsException>(() => handle());
    }

    [Fact]
    async Task CreateBranch_ShouldBeSuccessful_WhenCommandIsValid()
    {
        //Arrange
        var command = new CreateBranchCommand("grana1", new Guid("8e9b1cc3-35d6-4bf2-9f2c-9e00a21d94a5"), false, new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
        //Act

        var branchId = await _sender.Send(command);
        Branch? branch = await _context.Branches.FindAsync(branchId);

        //Assert

        branchId.ShouldBeOfType<Guid>();
        branch.ShouldNotBeNull();
    }


}

