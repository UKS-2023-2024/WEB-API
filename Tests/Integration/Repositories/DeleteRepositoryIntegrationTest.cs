using Application.Organizations.Commands.Delete;
using Application.Repositories.Commands.Delete;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Organizations;
using Shouldly;
using Tests.Integration.Setup;
using Tests.Unit.Organizations;

namespace Tests.Integration.Repositories;


[Collection("Sequential")]
public class DeleteRepositoryIntegrationTest: BaseIntegrationTest
{
    public DeleteRepositoryIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
   
    }  

    [Fact]
    public async Task DeleteRepository_ShouldBeSuccess_WhenCommandValid()
    {
        //Arrange
        var command = new DeleteRepositoryCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d94a5"));

        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.NotThrowAsync(() => handle());
    }
    
    [Fact]
    public async Task DeleteRepository_ShouldFail_WhenUserNotOwner()
    {
        //Arrange
        var command = new DeleteRepositoryCommand(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"));

        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<UnautorizedAccessException>(() => handle());
    }
}