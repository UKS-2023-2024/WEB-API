using Application.Shared;
using Application.Shared.Email;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Events;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Services;
using Domain.Organizations.Types;
using MediatR;

namespace Application.Organizations.Commands.SendInvite;

public class SendInviteCommandHandler: ICommandHandler<SendInviteCommand>
{
    private readonly IMediator _mediator;
    private readonly IPermissionService _permissionService;
    private readonly IOrganizationInviteRepository _organizationInviteRepository;
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    public SendInviteCommandHandler(
        IMediator mediator,
        IOrganizationMemberRepository organizationMemberRepository,
        IPermissionService permissionService,
        IOrganizationInviteRepository organizationInviteRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
        _permissionService = permissionService;
        _mediator = mediator;
        _organizationInviteRepository = organizationInviteRepository;
    }

    public async Task Handle(SendInviteCommand request, CancellationToken cancellationToken)
    {
        await _permissionService.ThrowIfNoPermission(new PermissionParams
        {
            Authorized = request.Authorized,
            OrganizationId = request.OrganizationId,
            Permission = "admin"
        });
        
        var member = await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.MemberId, request.OrganizationId);
        if (member is not null) 
            throw new AlreadyOrganizationMemberException();

        var existingInvitation = _organizationInviteRepository.FindByOrgAndMember(request.OrganizationId, request.MemberId);
        if (existingInvitation is not null)
             _organizationInviteRepository.Delete(existingInvitation);
        
        var invite = OrganizationInvite.Create(request.MemberId, request.OrganizationId);
        
        await _organizationInviteRepository.Create(invite);
        await _mediator.Publish(new OrganizationMemberInvitedEvent(invite.Id), default);
    }
}