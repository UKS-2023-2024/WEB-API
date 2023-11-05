using Application.Organizations.Commands.Create;
using Application.Repositories.Commands.Create;
using Domain.Auth;
using Domain.Exceptions;
using Domain.Exceptions.Repositories;
using Domain.Organizations;
using Domain.Repositories;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;


[Collection("Sequential")]
public class CreateRepositoryIntegrationTest : BaseIntegrationTest
{

    public CreateRepositoryIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {

    }

    [Fact]
    async Task CreateRepositoryForUser_ShouldBeSuccess_WhenCommandValid()
    {
        //Arrange
        var command = new CreateRepositoryCommand("repository", "test repository", false,
            Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), Guid.Empty);

        //Act
        Guid repositoryId = await _sender.Send(command);
        Repository repository = await _context.Repositories.FindAsync(repositoryId);

        //Assert
        repository.ShouldNotBeNull();
    }

    [Fact]
    async Task CreateRepositoryForOrganization_ShouldBeSuccess_WhenCommandValid()
    {
        //Arrange
        var command = new CreateRepositoryCommand("repository", "test repository", false,
            Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act
        Guid repositoryId = await _sender.Send(command);
        Repository repository = await _context.Repositories.FindAsync(repositoryId);

        //Assert
        repository.ShouldNotBeNull();
    }

    [Fact]
    async Task CreateRepositoryForUser_ShouldFail_WhenUserAlreadyHasRepositoryWithSameName()
    {
        //Arrange
        var command = new CreateRepositoryCommand("repo", "test repo", false,
            Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), Guid.Empty);

        //Act

        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<RepositoryWithThisNameExistsException>(() => handle());

    }

    [Fact]
    async Task CreateRepositoryForOrganization_ShouldFail_WhenOrganizationAlreadyHasRepositoryWithSameName()
    {
        //Arrange
        var command = new CreateRepositoryCommand("repo", "test repo", false,
            Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act

        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<RepositoryWithThisNameExistsException>(() => handle());

    }
}