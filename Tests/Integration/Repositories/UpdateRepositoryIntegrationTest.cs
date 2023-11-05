using Application.Auth.Commands.Update;
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
public class UpdateRepositoryIntegrationTest : BaseIntegrationTest
{

    public UpdateRepositoryIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {

    }

    [Fact]
    async Task UpdateUserRepository_ShouldBeSuccess_WhenCommandValid()
    {
        //Arrange
        var command = new UpdateRepositoryCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"),
            "izmenjeno ime", "test", false);

        //Act
        var repository = await _sender.Send(command);

        //Assert
        repository.ShouldNotBeNull();
    }

    [Fact]
    async Task UpdateUserRepository_ShouldBeFail_WhenUserAlreadyHasRepositoryWithSameName()
    {
        //Arrange
        var command = new UpdateRepositoryCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"), new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"),
            "repo3", "test", false);

        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<RepositoryWithThisNameExistsException>(() => handle());
    }

}