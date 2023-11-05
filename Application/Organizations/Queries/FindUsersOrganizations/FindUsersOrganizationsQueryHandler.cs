using Domain.Organizations;
using Domain.Organizations.Interfaces;
using MediatR;

namespace Application.Organizations.Queries.FindUserOrganizations;

public class FindUserOrganizationsQueryHandler: IRequestHandler<FindUserOrganizationsQuery, List<Organization>>
{
    private readonly IOrganizationMemberRepository _organizationMemberRepository;

    public FindUserOrganizationsQueryHandler(IOrganizationMemberRepository organizationMemberRepository)
    {
        _organizationMemberRepository = organizationMemberRepository;
    }

    public Task<List<Organization>> Handle(FindUserOrganizationsQuery request, CancellationToken cancellationToken)
    {
        return _organizationMemberRepository.FindUserOrganizations(request.UserId);
    }
}