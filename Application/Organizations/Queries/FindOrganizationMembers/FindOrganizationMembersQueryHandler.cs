using Domain.Organizations;
using Domain.Organizations.Exceptions;
using Domain.Organizations.Interfaces;
using MediatR;

namespace Application.Organizations.Queries.FindOrganizationMembers;

public class FindOrganizationMembersQueryHandler: IRequestHandler<FindOrganizationMembersQuery, IEnumerable<OrganizationMember>>
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;

    public FindOrganizationMembersQueryHandler(IOrganizationMemberRepository organizationMemberRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
    }
    
    public async Task<IEnumerable<OrganizationMember>> Handle(FindOrganizationMembersQuery request, CancellationToken cancellationToken)
    {
        var organizationMember =
            await _organizationMemberRepository.FindByUserIdAndOrganizationId(request.UserId, request.OrganizationId);
        OrganizationMember.ThrowIfDoesntExist(organizationMember);
        return await _organizationMemberRepository.FindOrganizationMembers(request.OrganizationId);
    }
}