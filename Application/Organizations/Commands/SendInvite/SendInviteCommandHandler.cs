using Application.Shared;
using Domain.Organizations;
using Domain.Organizations.Events;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Types;
using MediatR;

namespace Application.Organizations.Commands.SendInvite;

public class SendInviteCommandHandler: ICommandHandler<SendInviteCommand>
{
    private readonly IMediator _mediator;
    private readonly IOrganizationInviteRepository _organizationInviteRepository;
    private readonly IOrganizationMemberRepository _organizationMemberRepository;
    public SendInviteCommandHandler(
        IMediator mediator,
        IOrganizationMemberRepository organizationMemberRepository,
        IOrganizationInviteRepository organizationInviteRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
        _mediator = mediator;
        _organizationInviteRepository = organizationInviteRepository;
    }

    public async Task Handle(SendInviteCommand request, CancellationToken cancellationToken)
    {
        var sender = await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.Authorized, request.OrganizationId);
        OrganizationMember.ThrowIfDoesntExist(sender);
        sender.ThrowIfNoAdminPrivileges();
        
        var member = await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.MemberId, request.OrganizationId);
        if (member is not null && !member.Deleted) 
            throw new AlreadyOrganizationMemberException();

        var existingInvitation = _organizationInviteRepository.FindByOrgAndMember(request.OrganizationId, request.MemberId);
        if (existingInvitation is not null)
             _organizationInviteRepository.Delete(existingInvitation);
        var invite = OrganizationInvite.Create(request.MemberId, request.OrganizationId);
        
        await _organizationInviteRepository.Create(invite);
        await _mediator.Publish(new OrganizationMemberInvitedEvent(invite.Id), cancellationToken);
    }
}