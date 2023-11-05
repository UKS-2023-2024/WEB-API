using Application.Organizations.Commands.Delete;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Organizations;
using Shouldly;
using Tests.Integration.Setup;
using Tests.Unit.Organizations;

namespace Tests.Integration.Organizations;


[Collection("Sequential")]
public class DeleteOrganizationIntegrationTest: BaseIntegrationTest
{

    private Guid _organizationId;
    public DeleteOrganizationIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        _organizationId = organization.Id;
    }  

    [Fact]
    public async Task DeleteOrganization_ShouldBeSuccess_WhenCommandValid()
    {
        //Arrange
        var command = new DeleteOrganizationCommand(_organizationId,
            Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.NotThrowAsync(() => handle());
    }
    
    [Fact]
    public async Task DeleteOrganization_ShouldFail_WhenUserNotOwner()
    {
        //Arrange
        var command = new DeleteOrganizationCommand(_organizationId,
            Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"));

        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<UnauthorizedAccessException>(() => handle());
    }
}