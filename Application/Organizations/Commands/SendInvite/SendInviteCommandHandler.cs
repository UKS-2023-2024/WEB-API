using Application.Shared;
using Application.Shared.Email;
using Domain.Auth.Interfaces;
using Domain.Organizations;
using Domain.Organizations.Events;
using Domain.Organizations.Interfaces;
using Domain.Organizations.Services;
using Domain.Organizations.Types;
using MediatR;

namespace Application.Organizations.Commands.SendInvite;

public class SendInviteCommandHandler: ICommandHandler<SendInviteCommand>
{
    private readonly IMediator _mediator;
    private readonly IHashingService _hashingService;
    private readonly IOrganizationRepository _organizationRepository;
    private readonly IPermissionService _permissionService;
    private readonly IOrganizationInviteRepository _organizationInviteRepository;

    public SendInviteCommandHandler(
        IMediator mediator,
        IHashingService hashingService,
        IOrganizationRepository organizationRepository,
        IPermissionService permissionService,
        IOrganizationInviteRepository organizationInviteRepository)
    {
        _hashingService = hashingService;
        _organizationRepository = organizationRepository;
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
        
        var member = await _organizationRepository.FindMember(request.OrganizationId, request.MemberId);
        OrganizationMember.ThrowIfDoesntExist(member);

        var existingInvitation = _organizationInviteRepository.FindByOrgAndMember(request.OrganizationId, request.MemberId);
        if (existingInvitation is not null)
             _organizationInviteRepository.Delete(existingInvitation);
        
        var token = _hashingService.GenerateRandomToken();
        var invite = OrganizationInvite.Create(request.OrganizationId, request.MemberId, token);
        
        await _organizationInviteRepository.Create(invite);
        await _mediator.Publish(new OrganizationMemberInvitedEvent(invite), default);
    }
}