using Application.Shared;
using Domain.Organizations;

namespace Application.Organizations.Queries.FindOrganizationMembers;

public record FindOrganizationMembersQuery(Guid UserId,Guid OrganizationId) : IQuery<IEnumerable<OrganizationMember>>;