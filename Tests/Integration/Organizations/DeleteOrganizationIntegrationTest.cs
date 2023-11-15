using Application.Organizations.Commands.Delete;
using Domain.Organizations.Exceptions;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Organizations;


[Collection("Sequential")]
public class DeleteOrganizationIntegrationTest: BaseIntegrationTest
{

    private Guid _organizationId;
    public DeleteOrganizationIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        _organizationId = organization!.Id;
    }  

    [Fact]
    public async Task DeleteOrganization_ShouldBeSuccess_WhenCommandValid()
    {
        //Arrange
        
        var command = new DeleteOrganizationCommand(_organizationId,
            new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    [Fact]
    public async Task DeleteOrganization_ShouldFail_WhenUserNotOwner()
    {
        //Arrange
        var command = new DeleteOrganizationCommand(_organizationId,
            Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"));

        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<PermissionDeniedException>(Handle);
    }
    
}