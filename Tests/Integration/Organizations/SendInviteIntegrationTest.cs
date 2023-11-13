using Application.Organizations.Commands.SendInvite;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Organizations.Exceptions;
using Shouldly;
using Tests.Integration.Setup;

namespace Tests.Integration.Organizations;


[Collection("Sequential")]
public class SendInviteIntegrationTest: BaseIntegrationTest
{
    public SendInviteIntegrationTest(TestDatabaseFactory factory) : base(factory)
    {
    }


    [Fact]
    async Task SendInvite_ShouldPass()
    {

        //Arrange
        Guid authorized = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        Guid memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var inviteCommand = new SendInviteCommand(authorized, organization.Id, memberId);
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(inviteCommand);
        };
        
        //Assert
        await Should.NotThrowAsync(() => handle());
        var invite = _context.OrganizationInvites.FirstOrDefault(o =>
            o.OrganizationId.Equals(organization.Id) && o.UserId.Equals(memberId));
        invite.ShouldNotBeNull();
    }
    
    [Fact]
    async Task SendInvite_ShouldFail_WhenMemberAlreadyExists()
    {

        //Arrange
        Guid authorized = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a5");
        Guid memberId = new Guid("7e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a7");
        var organization = _context.Organizations.FirstOrDefault(o => o.Name.Equals("organization1"));
        var inviteCommand = new SendInviteCommand(authorized, organization.Id, memberId);
        //Act
        Func<Task> handle = async () =>
        {
            await _sender.Send(inviteCommand);
        };
        
        //Assert
        await Should.ThrowAsync<AlreadyOrganizationMemberException>(() => handle());
    }
}