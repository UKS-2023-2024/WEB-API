using Application.Organizations.Commands.SendInvite;
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
    private Mock<IPermissionService> _permissionService = new();
    private Mock<IOrganizationInviteRepository> _organizationInviteRepository = new();


    [Fact]
    async Task SendInvite_ShouldBeSuccess()
    {
        //Arrange
        var authorized = new Guid();
        var organizationId = new Guid();
        var memberId = new Guid();
        var role = OrganizationRole.Create("OWNER", "");
        var member = OrganizationMember.Create(memberId, organizationId,role);
        var invite = OrganizationInvite.Create(memberId, organizationId);

        _permissionService.Setup(o => 
            o.ThrowIfNoPermission(It.IsAny<PermissionParams>()));
        _organizationMemberRepository.Setup(o =>
            o.FindByUserIdAndOrganizationId(It.IsAny<Guid>(), It.IsAny<Guid>()));
        _organizationInviteRepository
            .Setup(o => 
                o.FindByOrgAndMember(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(invite);
        _organizationInviteRepository.Setup(
                o => o.Create(It.IsAny<OrganizationInvite>()))
            .ReturnsAsync(invite)
            .Verifiable();

        _mediator.Setup(o =>
            o.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()));
            
        var sendInviteCommand = new SendInviteCommand(
            authorized,
            organizationId,
            memberId);
            
        var sendInviteCommandHandler = new SendInviteCommandHandler(
            _mediator.Object,
            _organizationMemberRepository.Object,
            _permissionService.Object,
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
        var authorized = new Guid();
        var organizationId = new Guid();
        var memberId = new Guid();
        var role = OrganizationRole.Create("OWNER", "");
        var member = OrganizationMember.Create(memberId, organizationId,role);
        var invite = OrganizationInvite.Create(memberId, organizationId);

        _permissionService.Setup(o => 
            o.ThrowIfNoPermission(It.IsAny<PermissionParams>()));
        _organizationMemberRepository.Setup(o =>
                o.FindByUserIdAndOrganizationId(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .ReturnsAsync(member);
        _organizationInviteRepository
            .Setup(o => 
                o.FindByOrgAndMember(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(invite);
        _organizationInviteRepository.Setup(
                o => o.Create(It.IsAny<OrganizationInvite>()))
            .ReturnsAsync(invite);
        _mediator.Setup(o =>
            o.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()));
            
        var sendInviteCommand = new SendInviteCommand(
            authorized,
            organizationId,
            memberId);
            
        var sendInviteCommandHandler = new SendInviteCommandHandler(
            _mediator.Object,
            _organizationMemberRepository.Object,
            _permissionService.Object,
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
        var authorized = new Guid();
        var organizationId = new Guid();
        var memberId = new Guid();
        var role = OrganizationRole.Create("OWNER", "");
        var member = OrganizationMember.Create(memberId, organizationId,role);
        var invite = OrganizationInvite.Create(memberId, organizationId);

        _permissionService.Setup(o => 
            o.ThrowIfNoPermission(It.IsAny<PermissionParams>()));
        _organizationMemberRepository.Setup(o =>
            o.FindByUserIdAndOrganizationId(It.IsAny<Guid>(), It.IsAny<Guid>()));
        _organizationInviteRepository
            .Setup(o => 
                o.FindByOrgAndMember(It.IsAny<Guid>(), It.IsAny<Guid>()))
            .Returns(invite);
        _organizationInviteRepository.Setup(
                o => o.Create(It.IsAny<OrganizationInvite>()))
            .ReturnsAsync(invite);
        _mediator.Setup(o =>
            o.Publish(It.IsAny<INotification>(), It.IsAny<CancellationToken>()));
        _organizationInviteRepository.Setup(o => o.Delete(It.IsAny<OrganizationInvite>()))
            .Verifiable();
        
        var sendInviteCommand = new SendInviteCommand(
            authorized,
            organizationId,
            memberId);
            
        var sendInviteCommandHandler = new SendInviteCommandHandler(
            _mediator.Object,
            _organizationMemberRepository.Object,
            _permissionService.Object,
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
}