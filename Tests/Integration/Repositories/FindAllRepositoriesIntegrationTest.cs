using Application.Organizations.Commands.Create;
using Application.Repositories.Commands.Create;
using Application.Repositories.Queries.FindAllByOrganizationId;
using Application.Repositories.Queries.FindAllByOwnerId;
using Domain.Auth;
using Domain.Exceptions;
using Domain.Exceptions.Repositories;
using Domain.Organizations;
using Domain.Repositories;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;


[Collection("Sequential")]
public class FindAllRepositoriesByOrganizationIdIntegrationTest : BaseIntegrationTest
{

    public FindAllRepositoriesByOrganizationIdIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {

    }

    [Fact]
    async Task FindAllRepositoriesByOwnerId_ShouldReturnNotEmptyList()
    {
        //Arrange
        var query = new FindAllRepositoriesByOwnerIdQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act
        var repositories = await _sender.Send(query);

        //Assert
        repositories.ShouldNotBeEmpty();
    }

    [Fact]
    async Task FindAllRepositoriesByOrganizationId_ShouldReturnNotEmptyList()
    {
        //Arrange
        var query = new FindAllRepositoriesByOrganizationIdQuery(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act
        var repositories = await _sender.Send(query);

        //Assert
        repositories.ShouldNotBeEmpty();
    }
}
