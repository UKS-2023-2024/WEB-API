using Application.Organizations.Commands.ChangeOrganizationMemberRole;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Repositories.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Organizations;

[Collection("Sequential")]
public class ChangeOrganizationMemberRoleIntegrationTests : BaseIntegrationTest
{
    public ChangeOrganizationMemberRoleIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    
    [Fact]
    public async void ChangeOrganizationMemberRole_ShouldBeSuccess_WhenUserMemberAndOwnerHasPrivileges()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        
        var command = new ChangeOrganizationMemberRoleCommand(ownerId,
            memberId, organization!.Id,OrganizationMemberRole.MODERATOR);

        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.NotThrowAsync(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenSenderNotOrganizationMember()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var command = new ChangeOrganizationMemberRoleCommand(ownerId,
            memberId, organization!.Id,OrganizationMemberRole.MEMBER);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<OrganizationMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenSenderDoesNotHavePrivileges()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9211");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var command = new ChangeOrganizationMemberRoleCommand(ownerId,
            memberId, organization!.Id,OrganizationMemberRole.MEMBER);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<MemberHasNoPrivilegeException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenMemberToChangeNotMember()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var command = new ChangeOrganizationMemberRoleCommand(ownerId,
            memberId, organization!.Id,OrganizationMemberRole.MEMBER);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<OrganizationMemberNotFoundException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenMemberSameAsSender()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var command = new ChangeOrganizationMemberRoleCommand(ownerId,
            memberId, organization!.Id,OrganizationMemberRole.MEMBER);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<MemberCantChangeHimselfException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenSenderWantToChangeOwner1()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var command = new ChangeOrganizationMemberRoleCommand(ownerId,
            memberId, organization!.Id,OrganizationMemberRole.MEMBER);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<CantChangeOrganizationOwnerException>(Handle);
    }
    
    [Fact]
    public async void ChangeRepositoryMemberRole_ShouldFail_WhenSenderWantToChangeOwner2()
    {
        //Arrange
        var ownerId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d9211");
        var command = new ChangeOrganizationMemberRoleCommand(ownerId,
            memberId, organization!.Id,OrganizationMemberRole.OWNER);
        
        //Act
        async Task Handle() => await _sender.Send(command);

        //Assert
        await Should.ThrowAsync<CantChangeOrganizationOwnerException>(Handle);
    }
}