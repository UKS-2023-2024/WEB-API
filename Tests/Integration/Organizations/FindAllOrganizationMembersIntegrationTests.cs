using Application.Organizations.Queries.FindOrganizationMembers;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Organizations;

[Collection("Sequential")]
public class FindAllOrganizationMembersIntegrationTests:BaseIntegrationTest
{
    public FindAllOrganizationMembersIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void Handle_ShouldFail_WhenUserNotInOrganization()
    {
        //Arrange
        var organization = _context.Organizations.FirstOrDefault(org => org.Name.Equals("organization1"));
        var command = new FindOrganizationMembersQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"),
            organization!.Id);

        //Act
        async Task<IEnumerable<OrganizationMember>> Handle() => await _sender.Send(command);
        

        //Assert
        await Should.ThrowAsync<CantAccessOrganizationMembers>(Handle);
    }
    
    [Fact]
    public async void Handle_ShouldReturn3_WhenUserInOrganization()
    {
        //Arrange
        var organization = _context.Organizations.FirstOrDefault(org => org.Name.Equals("organization1"));
        var command = new FindOrganizationMembersQuery(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5"),
            organization!.Id);

        //Act
        async Task<IEnumerable<OrganizationMember>> Handle() => await _sender.Send(command);
        

        //Assert
        var res = await Handle();
        res.Count().ShouldBe(3);
    }
}