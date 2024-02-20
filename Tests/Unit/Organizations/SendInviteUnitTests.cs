using Application.Organizations.Commands.SendInvite;
using Domain.Auth;
using Domain.Auth.Enums;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;
using MediatR;
using Moq;
using Shouldly;

namespace Tests.Unit.Organizations;

public class SendInviteUnitTests
{
    private Mock<IMediator> _mediator = new();
    private Mock<IOrganizationMemberRepository> _organizationMemberRepository = new();
    private Mock<IOrganizationInviteRepository> _organizationInviteRepository = new();


    [Fact]
    async Task SendInvite_ShouldBeSuccess()
    {
        //Arrange
        var user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        var user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        var organization = Organization.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"),"orgName", "dusanjanosevic007@gmail.com",
            new List<User>(),user1);
        var orgMember = organization.Members.First(member => member.MemberId.Equals(user1.Id));
        orgMember = OverrideOrganizationId(orgMember, organization.Id);
        
        _organizationMemberRepository.Setup(o =>
            o.FindByUserIdAndOrganizationId(orgMember.MemberId, orgMember.OrganizationId)).ReturnsAsync(orgMember);

        _mediator.Setup(o =>
            o.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()));
            
        var sendInviteCommand = new SendInviteCommand(
            user1.Id,
            organization.Id,
            user2.Id);
            
        var sendInviteCommandHandler = new SendInviteCommandHandler(
            _mediator.Object,
            _organizationMemberRepository.Object,
            _organizationInviteRepository.Object
            );
        
        //Act
        Func<Task> handle = async () =>
        {
            await sendInviteCommandHandler.Handle(sendInviteCommand, default);

        };

        //Assert
        await Should.NotThrowAsync(handle);
        _organizationInviteRepository.Verify();
    }
    
    [Fact]
    async Task SendInvite_ShouldFail_WhenMemberAlreadyExists()
    {
        //Arrange
        var user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        var user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        var organization = Organization.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"),"orgName", "dusanjanosevic007@gmail.com",
            new List<User>(),user1);
        var orgMember = organization.Members.First(member => member.MemberId.Equals(user1.Id));
        orgMember = OverrideOrganizationId(orgMember, organization.Id);
        var organizationMember2 = organization.AddMember(user2);
        var invite = OrganizationInvite.Create(user2.Id, organization.Id);
        
        _organizationMemberRepository.Setup(o =>
            o.FindByUserIdAndOrganizationId(orgMember.MemberId, orgMember.OrganizationId)).ReturnsAsync(orgMember);
        _organizationMemberRepository.Setup(o =>
            o.FindByUserIdAndOrganizationId(organizationMember2.MemberId, organizationMember2.OrganizationId)).ReturnsAsync(organizationMember2);
        _organizationInviteRepository
            .Setup(o => 
                o.FindByOrgAndMember(invite.OrganizationId, invite.UserId))
            .Returns(invite);
        _organizationInviteRepository.Setup(
                o => o.Create(It.IsAny<OrganizationInvite>()))
            .ReturnsAsync(invite);
        _mediator.Setup(o =>
            o.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()));
        _organizationInviteRepository.Setup(o => o.Delete(It.IsAny<OrganizationInvite>()))
            .Verifiable();
        
        var sendInviteCommand = new SendInviteCommand(
            user1.Id,
            organization.Id,
            user2.Id);
            
        var sendInviteCommandHandler = new SendInviteCommandHandler(
            _mediator.Object,
            _organizationMemberRepository.Object,
            _organizationInviteRepository.Object
        );
        
        //Act
        Func<Task> handle = async () =>
        {
            await sendInviteCommandHandler.Handle(sendInviteCommand, default);

        };

        //Assert
        await Should.ThrowAsync<AlreadyOrganizationMemberException>(handle);
    }
    
    [Fact]
    async Task SendInvite_ShouldPass_WhenInviteAlreadyExists()
    {
        //Arrange
        var user1 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a8"), "dusanjanosevic007@gmail.com", "full name", "username1", "password", UserRole.USER);
        var user2 = User.Create(new Guid("8e9b1cc0-35d3-4bf2-9f2c-5e00a21d92a9"), "dusan.janosevic123@gmail.com", "full name", "username2", "password", UserRole.USER);
        var organization = Organization.Create(new Guid("8e9b1cc2-ffaa-4bf2-9f2c-5e00a21d92a9"),"orgName", "dusanjanosevic007@gmail.com",
            new List<User>(),user1);
        var orgMember = organization.Members.First(member => member.MemberId.Equals(user1.Id));
        orgMember = OverrideOrganizationId(orgMember, organization.Id);
        var invite = OrganizationInvite.Create(user2.Id, organization.Id);
        
        _organizationMemberRepository.Setup(o =>
            o.FindByUserIdAndOrganizationId(orgMember.MemberId, orgMember.OrganizationId)).ReturnsAsync(orgMember);
        _organizationInviteRepository
            .Setup(o => 
                o.FindByOrgAndMember(invite.OrganizationId, invite.UserId))
            .Returns(invite);
        _organizationInviteRepository.Setup(
                o => o.Create(It.IsAny<OrganizationInvite>()))
            .ReturnsAsync(invite);
        _mediator.Setup(o =>
            o.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()));
        _organizationInviteRepository.Setup(o => o.Delete(It.IsAny<OrganizationInvite>()))
            .Verifiable();
        
        var sendInviteCommand = new SendInviteCommand(
            user1.Id,
            organization.Id,
            user2.Id);
            
        var sendInviteCommandHandler = new SendInviteCommandHandler(
            _mediator.Object,
            _organizationMemberRepository.Object,
            _organizationInviteRepository.Object
        );
        
        //Act
        Func<Task> handle = async () =>
        {
            await sendInviteCommandHandler.Handle(sendInviteCommand, default);
        };

        await Should.NotThrowAsync(handle);
        _organizationInviteRepository.Verify();
    }
    
    private OrganizationMember OverrideOrganizationId(OrganizationMember obj, Guid id)
    {
        var propertyInfo = typeof(OrganizationMember).GetProperty("OrganizationId");
        if (propertyInfo == null) return obj;
        propertyInfo.SetValue(obj, id);
        return obj;
    }
}