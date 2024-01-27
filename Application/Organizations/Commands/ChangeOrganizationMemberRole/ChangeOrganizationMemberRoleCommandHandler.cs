using Application.Shared;
using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;

namespace Application.Organizations.Commands.ChangeOrganizationMemberRole;

public class ChangeOrganizationMemberRoleCommandHandler : ICommandHandler<ChangeOrganizationMemberRoleCommand>
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;

    public ChangeOrganizationMemberRoleCommandHandler(IOrganizationMemberRepository organizationMemberRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
    }

    public async Task Handle(ChangeOrganizationMemberRoleCommand request, CancellationToken cancellationToken)
    {
        var owner = await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.OwnerId,request.OrganizationId);
        OrganizationMember.ThrowIfDoesntExist(owner);
        owner.ThrowIfNoAdminPrivileges();

        var member =
            await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.MemberId, request.OrganizationId);
        OrganizationMember.ThrowIfDoesntExist(member);
        member.ThrowIfSameAs(owner);
        
        if (request.Role == OrganizationMemberRole.OWNER || member.HasRole(OrganizationMemberRole.OWNER))
            throw new CantChangeOrganizationOwnerException();
        
        member.SetRole(request.Role);
        _organizationMemberRepository.Update(member);

    }
}