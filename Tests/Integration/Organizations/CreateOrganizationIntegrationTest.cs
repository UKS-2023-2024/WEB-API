using Application.Organizations.Commands.Create;
using Domain.Auth;
using Domain.Exceptions;
using Domain.Organizations;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Organizations;


[Collection("Sequential")]
public class CreateOrganizationIntegrationTest: BaseIntegrationTest
{
    
    public CreateOrganizationIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
        
    }

    [Fact]
    async Task CreateOrganization_ShouldBeSuccess_WhenCommandValid()
    {
        //Arrange
        var command = new CreateOrganizationCommand("organization", "sara@gmail.com", new List<Guid>(),
            Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));

        //Act
        Guid organizationId = await _sender.Send(command);
        Organization organization = await _context.Organizations.FindAsync(organizationId);

        //Assert
        organization.ShouldNotBeNull();
    }

    [Fact]
    async Task CreateOrganization_ShouldFail_WhenOrganizationNameNotUnique()
    {
        //Arrange
        var command = new CreateOrganizationCommand("organization1", "sara@gmail.com", new List<Guid>(),
            Guid.Parse("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"));
        
        //Act
        
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };

        //Assert
        await Should.ThrowAsync<OrganizationWithThisNameExistsException>(() => handle());
        
    }
}