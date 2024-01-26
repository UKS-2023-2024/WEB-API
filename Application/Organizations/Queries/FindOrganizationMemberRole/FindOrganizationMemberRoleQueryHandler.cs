using Domain.Organizations;
using Domain.Organizations.Interfaces;
using MediatR;

namespace Application.Organizations.Queries.FindOrganizationMemberRole;

public class FindOrganizationMemberRoleQueryHandler: IRequestHandler<FindOrganizationMemberRoleQuery, OrganizationMemberRole>
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;

    public FindOrganizationMemberRoleQueryHandler(IOrganizationMemberRepository organizationMemberRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
    }

    public async Task<OrganizationMemberRole> Handle(FindOrganizationMemberRoleQuery request, CancellationToken cancellationToken)
    {
        var organizationMember = await _organizationMemberRepository
            .FindByUserIdAndOrganizationId(request.UserId, request.OrganizationId);
        OrganizationMember.ThrowIfDoesntExist(organizationMember);
        return organizationMember.Role;
    }
}