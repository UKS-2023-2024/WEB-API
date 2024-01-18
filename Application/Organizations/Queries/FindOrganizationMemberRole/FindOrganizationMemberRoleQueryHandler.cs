using Domain.Organizations.Interfaces;
using MediatR;

namespace Application.Organizations.Queries.FindOrganizationMemberRole;

public class FindOrganizationMemberRoleQueryHandler: IRequestHandler<FindOrganizationMemberRoleQuery, string?>
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;

    public FindOrganizationMemberRoleQueryHandler(IOrganizationMemberRepository organizationMemberRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
    }

    public async Task<string?> Handle(FindOrganizationMemberRoleQuery request, CancellationToken cancellationToken)
    {
        var organizationMember = await _organizationMemberRepository
            .FindByUserIdAndOrganizationId(request.UserId, request.OrganizationId);
        return organizationMember.Role.Name;
    }
}