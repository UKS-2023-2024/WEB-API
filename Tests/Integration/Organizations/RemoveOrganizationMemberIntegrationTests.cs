using Application.Organizations.Commands.RemoveOrganizationMember;
using Domain.Organizations.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Organizations;

[Collection("Sequential")]
public class RemoveOrganizationMemberIntegrationTests: BaseIntegrationTest
{
    public RemoveOrganizationMemberIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    async Task RemoveOrganizationMember_ShouldFail_WhenUserToRemoveNotInOrganization()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var removeCommand = new RemoveOrganizationMemberCommand(ownerId, memberId, organization!.Id);
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.ThrowAsync<OrganizationMemberNotFoundException>(Handle);
    }
    
    [Fact]
    async Task RemoveOrganizationMember_ShouldFail_WhenOrganizationMemberNotInChosenOrganization()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization2"));
        var removeCommand = new RemoveOrganizationMemberCommand(ownerId, memberId, organization!.Id);
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.ThrowAsync<OrganizationMemberNotFoundException>(Handle);
    }
    
    [Fact]
    async Task RemoveOrganizationMember_ShouldBeSuccess_WhenUserMemberOfOrganization()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var organizationMember = _context.OrganizationMembers.FirstOrDefault(m =>
            m.MemberId.Equals(new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7"))
                              && m.OrganizationId.Equals(organization!.Id));
        var removeCommand = new RemoveOrganizationMemberCommand(ownerId, organizationMember!.Id, organization!.Id);
        //Act
        async Task Handle() => await _sender.Send(removeCommand);

        //Assert
        await Should.NotThrowAsync(Handle);
    }

}