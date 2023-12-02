using Application.Repositories.Queries.DidUserStarRepository;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Repositories;

[Collection("Sequential")]
public class DidUserStarRepositoryIntegrationTest : BaseIntegrationTest
{
    public DidUserStarRepositoryIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    async Task DidUserStarRepository_ShouldReturnTrue()
    {
        //Arrange
        var query = new DidUserStarRepositoryQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"),new Guid("8e9b1cc1-35d3-4bf2-9f2c-5e00a21d14a5"));

        //Act
        var didUserStar = await _sender.Send(query);

        //Assert
        didUserStar.ShouldBeTrue();
    }
    
    [Fact]
    async Task DidUserStarRepository_ShouldReturnFalse()
    {
        //Arrange
        var query = new DidUserStarRepositoryQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),new Guid("8e9b1cc1-35d3-4bf2-9f2c-5e00a21d14a5"));

        //Act
        var didUserStar = await _sender.Send(query);

        //Assert
        didUserStar.ShouldBeFalse();
    }
}