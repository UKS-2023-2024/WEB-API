using System.Reflection;
using Application.Organizations.Commands.AcceptInvite;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Organizations;

[Collection("Sequential")]
public class AcceptInviteIntegrationTests: BaseIntegrationTest
{
    public AcceptInviteIntegrationTests(TestDatabaseFactory factory) : base(factory)
    {
    }
    private OrganizationInvite OverrideDate(OrganizationInvite invite, DateTime date)
    {
        PropertyInfo propertyInfo = typeof(OrganizationInvite).GetProperty("ExpiresAt");
        if (propertyInfo == null) return invite;
        propertyInfo.SetValue(invite, date);
        return invite;
    }

    [Fact]
    async Task AcceptInvitation_ShouldPass()
    {
        //Arrange
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var authorized = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var invite = OrganizationInvite.Create(memberId, organization.Id);
        await _context.OrganizationInvites.AddAsync(invite);
        await _context.SaveChangesAsync();
        var command = new AcceptInviteCommand(memberId, invite.Id);
        
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };
        
        //Assert
        await Should.NotThrowAsync(handle);
        var existingInvite = _context.OrganizationInvites.FirstOrDefault(o => o.Id.Equals(invite.Id));
        existingInvite.ShouldBeNull();
        var member = _context.OrganizationMembers.FirstOrDefault(mem =>
            mem.OrganizationId.Equals(organization.Id) && mem.MemberId.Equals(memberId));
        member.ShouldNotBeNull();
    }
    
    [Fact]
    async Task AcceptInvitation_ShouldFail_WhenInviteDoesntExist()
    {
        //Arrange
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var authorized = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));

        var command = new AcceptInviteCommand(memberId, new Guid());
        
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };
        
        //Assert
        await Should.ThrowAsync<OrganizationInviteNotFoundException>(handle);
    }
    
    [Fact]
    async Task AcceptInvitation_ShouldFail_WhenInviteHasExpired()
    {
        //Arrange
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var authorized = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var invite = OrganizationInvite.Create(memberId, organization.Id);
        invite = OverrideDate(invite, DateTime.Now.AddDays(-1).ToUniversalTime());
        await _context.OrganizationInvites.AddAsync(invite);
        await _context.SaveChangesAsync();
        var command = new AcceptInviteCommand(memberId, invite.Id);
        
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };
        
        //Assert
        await Should.ThrowAsync<InvitationExpiredException>(handle);
    }
    
    [Fact]
    async Task AcceptInvitation_ShouldFail_WhenOwnerIsNotSame()
    {
        //Arrange
        var memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var authorized = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var invite = OrganizationInvite.Create(memberId, organization.Id);
        await _context.OrganizationInvites.AddAsync(invite);
        await _context.SaveChangesAsync();
        var command = new AcceptInviteCommand(authorized, invite.Id);
        
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(command);
        };
        
        //Assert
        await Should.ThrowAsync<NotInviteOwnerException>(handle);
    }
}