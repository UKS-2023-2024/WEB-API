using Application.Organizations.Commands.Create;
using Domain.Organizations;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Organizations;

public class CreateIntegrationTest: BaseIntegrationTest
{
    public CreateIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }

    [Fact]
    async Task CreateOrganization_ShouldBeSuccess_WhenCommandValid()
    {
        //Arrange
        var command = new CreateOrganizationCommand("organization", "sara@gmail.com", new List<Guid>(),
            Guid.Parse("705a6c69-5b51-4156-b4cc-71e8dd111579"));
        Console.WriteLine("command");

        //Act
        Guid organizationId = await _sender.Send(command);
        Organization organization = await _context.Organizations.FindAsync(organizationId);

        //Assert
        organization.ShouldNotBeNull();
    }
}