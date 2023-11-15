using Application.Repositories.Queries.FindAllByOrganizationId;
using Application.Repositories.Queries.FindAllByOwnerId;
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
    async Task FindAllRepositoriesByOwnerId_ShouldReturnNonEmptyList()
    {
        //Arrange
        var query = new FindAllRepositoriesByOwnerIdQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act
        var repositories = await _sender.Send(query);

        //Assert
        repositories.ShouldNotBeEmpty();
    }

    [Fact]
    async Task FindAllRepositoriesByOrganizationId_ShouldReturnNonEmptyList()
    {
        //Arrange
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var query = new FindAllRepositoriesByOrganizationIdQuery(organization!.Id);

        //Act
        var repositories = await _sender.Send(query);

        //Assert
        repositories.ShouldNotBeEmpty();
    }
}
